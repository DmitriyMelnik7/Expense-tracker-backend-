using ExpenseTracker.Api.Database;
using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Account.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Entities;
using ExpenseTracker.Api.Exceptions;
using ExpenseTracker.Api.Extensions.QueryExtensions;
using ExpenseTracker.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services.Implementations
{
    public class AccountService(ApplicationDbContext dbContext, IValidator<AccountPatchDto> validator) : IAccountService
    {
        public async Task<PagedResult<AccountDto>> GetAllAsync(
            QueryParameters<AccountFilter, AccountSort> queryParameters,
            string userId,
            CancellationToken ct)
        {
            var query = dbContext.Accounts
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .ApplyFilter(queryParameters.Filter!)
                .ApplySort(queryParameters.Sort!)
                .Select(a => a.ToDto());

            var pagedResult = await query
                .ToPagedResultAsync(
                    queryParameters.Pagination.PageNumber,
                    queryParameters.Pagination.PageSize,
                    ct);

            return pagedResult;
        }

        public async Task<AccountDto> GetByIdAsync(string id, string userId, CancellationToken ct)
        {
            var account = await dbContext.Accounts
                .AsNoTracking()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);
            if (account is null)
                throw new NotFoundException("Account was not found");

            if (account.Status != AccountStatus.Active)
                throw new ConflictException("Account with this id is not active");

            return account.ToDto();
        }

        public async Task<AccountDto> CreateAsync(CreateAccountDto dto, string userId, CancellationToken ct)
        {
            var account = dto.ToEntity(userId);
            dbContext.Accounts.Add(account);

            await dbContext.SaveChangesAsync(ct);

            return account.ToDto();
        }

        public async Task<AccountDto> UpdateAsync(string id, JsonPatchDocument<AccountPatchDto> patch, string userId, CancellationToken ct)
        {
            var account = await dbContext.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);

            if (account is null)
                throw new NotFoundException("Account was not found");

            if (account.Status != AccountStatus.Active)
                throw new ConflictException("Unable to update inactive account");

            var patchDto = new AccountPatchDto
            {
                Name = account.Name
            };

            patch.ApplyTo(patchDto, error =>
            {
                throw new Exceptions.ValidationException(error.ErrorMessage);
            });

            var validationResult = await validator.ValidateAsync(patchDto, ct);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.ToString());

            account.Name = patchDto.Name;

            await dbContext.SaveChangesAsync(ct);

            return account.ToDto();
        }

        public async Task CloseAsync(string id, string userId, CancellationToken ct)
        {
            var account = await dbContext.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);

            if (account is null)
                throw new NotFoundException("Account was not found");

            if (account.Status == AccountStatus.Closed)
                return;

            //if (await dbContext.Transactions
            //    .IgnoreQueryFilters()
            //    .AnyAsync(t => t.AccountId == id, ct))
            //{
            //    throw new ConflictException("Unable to delete account with transactions");
            //}

            account.Status = AccountStatus.Closed;
            account.ClosedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(ct);
        }
    }
}
