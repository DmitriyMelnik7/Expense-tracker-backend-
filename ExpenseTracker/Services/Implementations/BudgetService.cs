using ExpenseTracker.Api.Database;
using ExpenseTracker.Api.DTOs.Budget;
using ExpenseTracker.Api.DTOs.Budget.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Exceptions;
using ExpenseTracker.Api.Extensions.QueryExtensions;
using ExpenseTracker.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services.Implementations
{
    public class BudgetService(ApplicationDbContext dbContext, IValidator<BudgetPatchDto> validator) : IBudgetService
    {
        public async Task<PagedResult<BudgetDto>> GetAllAsync(QueryParameters<BudgetFilter, BudgetSort> queryParameters, string userId, CancellationToken ct)
        {
            var query = dbContext.Budgets
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .ApplyFilter(queryParameters.Filter!)
                .ApplySort(queryParameters.Sort!)
                .Include(b => b.Category)
                .Select(b => b.ToDto());

            var pagedResult = await query
                .ToPagedResultAsync(
                    queryParameters.Pagination.PageNumber,
                    queryParameters.Pagination.PageSize,
                    ct);

            return pagedResult;
        }

        public async Task<BudgetDto> GetByIdAsync(string id, string userId, CancellationToken ct)
        {
            var budget = await dbContext.Budgets
                .AsNoTracking()
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId, ct);

            if (budget is null)
                throw new NotFoundException("Budget was not found");

            return budget.ToDto();
        }

        public async Task<BudgetDto> CreateAsync(CreateBudgetDto dto, string userId, CancellationToken ct)
        {
            if (await dbContext.Budgets.AnyAsync(c =>
                c.UserId == userId &&
                c.CategoryId == dto.CategoryId &&
                c.Month == dto.Month &&
                c.Year == dto.Year, ct))
            {
                throw new ConflictException("Budget with same category, year and month already exists");
            }

            if (!await dbContext.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId, ct))
                throw new NotFoundException("Category was not found");

            var budget = dto.ToEntity(userId);

            dbContext.Budgets.Add(budget);

            await dbContext.SaveChangesAsync(ct);

            await dbContext.Entry(budget).Reference(b => b.Category).LoadAsync(ct);

            return budget.ToDto();
        }

        public async Task<BudgetDto> UpdateAsync(string id, JsonPatchDocument<BudgetPatchDto> patch, string userId, CancellationToken ct)
        {
            var budget = await dbContext.Budgets.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId, ct);
            if (budget is null)
                throw new NotFoundException("Budget was not found");

            //budget.UpdateFromDto(dto);

            var patchDto = new BudgetPatchDto
            {
                Amount = budget.Amount,
                Month = budget.Month,
                Year = budget.Year
            };

            patch.ApplyTo(patchDto, error =>
            {
                throw new Exceptions.ValidationException(error.ErrorMessage);
            });

            var validationResult = await validator.ValidateAsync(patchDto, ct);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.ToString());

            budget.Amount = (decimal)patchDto.Amount;
            budget.Month = (int)patchDto.Month;
            budget.Year = (int)patchDto.Year;

            await dbContext.SaveChangesAsync(ct);

            await dbContext.Entry(budget).Reference(b => b.Category).LoadAsync(ct);

            return budget.ToDto();
        }

        public async Task DeleteAsync(string id, string userId, CancellationToken ct)
        {
            var budget = await dbContext.Budgets.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId, ct);
            if (budget is null)
                throw new NotFoundException("Budget was not found");

            dbContext.Remove(budget);

            await dbContext.SaveChangesAsync(ct);
        }

    }
}
