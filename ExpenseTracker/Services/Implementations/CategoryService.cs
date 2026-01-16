using ExpenseTracker.Api.Database;
using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Category;
using ExpenseTracker.Api.DTOs.Category.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Exceptions;
using ExpenseTracker.Api.Extensions.QueryExtensions;
using ExpenseTracker.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services.Implementations
{
    public sealed class CategoryService(ApplicationDbContext dbContext, IValidator<CategoryPatchDto> validator) : ICategoryService
    {
        public async Task<PagedResult<CategoryDto>> GetAllAsync(QueryParameters<CategoryFilter, CategorySort> queryParameters, string userId, CancellationToken ct)
        {
            var query = dbContext.Categories
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .ApplyFilter(queryParameters.Filter!)
                .ApplySort(queryParameters.Sort!)
                .Include(c => c.ParentCategory)
                .Select(c => c.ToDto());

            var result = await query.ToPagedResultAsync(
                queryParameters.Pagination.PageNumber,
                queryParameters.Pagination.PageSize,
                ct);

            return result;
        }
        public async Task<CategoryDto> GetByIdAsync(string id, string userId, CancellationToken ct)
        {
            var category = await dbContext.Categories
                .AsNoTracking()
                .Where(c => c.Id == id && c.UserId == userId)
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(ct);
            if (category is null)
                throw new NotFoundException("Category was not found");

            return category.ToDto();
        }
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, string userId, CancellationToken ct)
        {
            if (await dbContext.Categories.AnyAsync(c =>
                c.UserId == userId &&
                c.Name == dto.Name &&
                c.ParentCategoryId == dto.ParentCategoryId, ct))
            {
                throw new ConflictException("Category with the same name already exists");
            }

            if (dto.ParentCategoryId is not null &&
                !await dbContext.Categories.AnyAsync(c => c.Id == dto.ParentCategoryId && c.UserId == userId, ct))
            {
                throw new NotFoundException("Parent category not found");
            }

            var category = dto.ToEntity(userId);

            dbContext.Categories.Add(category);

            await dbContext.SaveChangesAsync(ct);

            if(category.ParentCategoryId is not null)
                await dbContext.Entry(category).Reference(c => c.ParentCategory).LoadAsync(ct);

            return category.ToDto();
        }
        public async Task<CategoryDto> UpdateAsync(string id, JsonPatchDocument<CategoryPatchDto> patch, string userId, CancellationToken ct)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, ct);
            if (category == null)
                throw new NotFoundException("Category was not found");

            var patchDto = new CategoryPatchDto
            {
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };

            patch.ApplyTo(patchDto);

            await ValidateAsync(id, patchDto, userId, ct);

            var validationResult = await validator.ValidateAsync(patchDto, ct);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.ToString());

            category.Name = patchDto.Name;
            category.ParentCategoryId = patchDto.ParentCategoryId;

            await dbContext.SaveChangesAsync(ct);

            await dbContext.Entry(category).Reference(c => c.ParentCategory).LoadAsync(ct);

            return category.ToDto();
        }

        public async Task DeleteAsync(string id, string userId, CancellationToken ct)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId, ct);
            if (category is null)
                throw new NotFoundException("Category was not found");

            if (await dbContext.Categories.AnyAsync(c => c.ParentCategoryId == id))
                throw new ConflictException("Unable to delete category with subcategories");

            if (await dbContext.Budgets.AnyAsync(b => b.CategoryId == id, ct))
                throw new ConflictException("Unable to delete category with budgets");

            dbContext.Categories.Remove(category);

            await dbContext.SaveChangesAsync(ct);
        }

        private async Task ValidateAsync(string id, CategoryPatchDto dto, string userId, CancellationToken ct)
        {
            if (dto.ParentCategoryId is not null &&
                !await dbContext.Categories.AnyAsync(c => c.Id == dto.ParentCategoryId && c.UserId == userId, ct))
            {
                throw new NotFoundException("Parent category not found");
            }

            var currentParentId = dto.ParentCategoryId;
            HashSet<string> visited = new HashSet<string>();

            while(currentParentId is not null)
            {
                if (currentParentId == id)
                    throw new BusinessException("Category cannot be parent of itself");

                if (!visited.Add(currentParentId))
                    throw new BusinessException("Category hierarchy contains a cycle");

                var parent = await dbContext.Categories
                    .AsNoTracking()
                    .Where(c => c.Id == currentParentId)
                    .Select(c => c.ParentCategoryId)
                    .FirstOrDefaultAsync(ct);

                currentParentId = parent;
            }

            if (dto.Name is not null &&
                await dbContext.Categories.AnyAsync(c => c.Name == dto.Name &&
                    c.UserId == userId &&
                    c.Id != id &&
                    c.ParentCategoryId == dto.ParentCategoryId, ct))
            {
                throw new ConflictException("Category with the same name already exists");
            }
        }
    }
}
