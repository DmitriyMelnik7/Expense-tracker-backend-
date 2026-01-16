using Azure;
using ExpenseTracker.Api.Database;
using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.DTOs.Transaction;
using ExpenseTracker.Api.DTOs.Transaction.Sorting;
using ExpenseTracker.Api.Entities;
using ExpenseTracker.Api.Exceptions;
using ExpenseTracker.Api.Extensions.QueryExtensions;
using ExpenseTracker.Api.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Services.Implementations
{
    public class TransactionService(ApplicationDbContext dbContext, IValidator<TransactionPatchDto> validator) : ITransactionService
    {
        public async Task<PagedResult<TransactionDto>> GetAllAsync(
            QueryParameters<TransactionFilter, TransactionSort> queryParameters,
            string userId, 
            CancellationToken ct)
        {
            var query = dbContext.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .ApplyFilter(queryParameters.Filter!)
                .ApplySort(queryParameters.Sort!)
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Select(t => t.ToDto());

            var result = await query.ToPagedResultAsync(
                queryParameters.Pagination.PageNumber, 
                queryParameters.Pagination.PageSize, 
                ct);

            return result;
        }

        public async Task<TransactionDto> GetByIdAsync(string id, string userId, CancellationToken ct)
        {
            var transaction = await dbContext.Transactions
                .AsNoTracking()
                .Where(t => t.Id == id && t.UserId == userId)
                .Include(t => t.Account)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(ct);

            if (transaction is null)
            {
                throw new NotFoundException("Transaction was not found");
            }

            return transaction.ToDto();
        }

        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto, string userId, CancellationToken ct)
        {
            using var dbTransaction = await dbContext.Database.BeginTransactionAsync(ct);

            var account = await dbContext.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == dto.AccountId && a.UserId == userId, ct);

            if (account is null)
                throw new NotFoundException("Account not found");

            if (account.Status != AccountStatus.Active)
                throw new ConflictException("Unable to create transaction with inactive account");

            var category = await dbContext.Categories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.UserId == userId, ct);

            if (category is null)
                throw new NotFoundException("Category not found");

            var transaction = dto.ToEntity(userId);

            if (category.CategoryType == CategoryType.Expense)
                account.Balance -= transaction.Amount;
            else if (category.CategoryType == CategoryType.Income)
                account.Balance += transaction.Amount;

            dbContext.Transactions.Add(transaction);

            await dbContext.SaveChangesAsync(ct);
            await dbTransaction.CommitAsync();

            await dbContext.Entry(transaction).Reference(t => t.Category).LoadAsync(ct);
            await dbContext.Entry(transaction).Reference(t => t.Account).LoadAsync(ct);

            return transaction.ToDto();
        }

        public async Task<TransactionDto> UpdateAsync(string id, JsonPatchDocument<TransactionPatchDto> patch, string userId, CancellationToken ct)
        {
            var transaction = await dbContext.Transactions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, ct);
            if (transaction is null)
                throw new NotFoundException("Transaction was not found");

            if (transaction.IsDeleted)
                throw new ConflictException("Unable to update deleted transaction");

            var patchDto = new TransactionPatchDto
            {
                AccountId = transaction.AccountId,
                CategoryId = transaction.CategoryId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Description = transaction.Description,
                ReceiptPath = transaction.ReceiptPath
            };

            patch.ApplyTo(patchDto, error =>
            {
                throw new Exceptions.ValidationException(error.ErrorMessage);
            });

            var validationResult = await validator.ValidateAsync(patchDto, ct);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.ToString());

            await ValidatePatchAsync(patchDto, userId, ct);

            transaction.Amount = patchDto.Amount ?? transaction.Amount;
            transaction.AccountId = patchDto.AccountId ?? transaction.AccountId;
            transaction.CategoryId = patchDto.CategoryId ?? transaction.CategoryId;
            transaction.Date = patchDto.Date ?? transaction.Date;
            transaction.Description = patchDto.Description ?? transaction.Description;
            transaction.ReceiptPath = patchDto.ReceiptPath ?? transaction.ReceiptPath;

            await dbContext.SaveChangesAsync(ct);

            await dbContext.Entry(transaction).Reference(t => t.Account).LoadAsync(ct);
            await dbContext.Entry(transaction).Reference(t => t.Category).LoadAsync(ct);

            return transaction.ToDto();
        }

        public async Task DeleteAsync(
            string id,
            string userId,
            CancellationToken ct)
        {
            using var dbTransaction = await dbContext.Database.BeginTransactionAsync(ct);

            var transaction = await dbContext.Transactions
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId, ct);

            if (transaction is null)
                throw new NotFoundException("Transaction was not found");

            if (transaction.IsDeleted) 
                return;

            var account = await dbContext.Accounts
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == transaction.AccountId, ct);

            if (account is null)
                throw new BusinessException("Account was not found for transaction");

            var category = await dbContext.Categories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == transaction.CategoryId, ct);

            if (category is null)
                throw new BusinessException("Category was not found for transaction");

            if (category.CategoryType == CategoryType.Expense)
                account.Balance += transaction.Amount;
            else if (category.CategoryType == CategoryType.Income)
                account.Balance -= transaction.Amount;

            transaction.IsDeleted = true;
            transaction.DeletedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync(ct);
            await dbTransaction.CommitAsync();
        }



        //methods
        public async Task ValidatePatchAsync(
            TransactionPatchDto dto,
            string userId,
            CancellationToken ct)
        {
            if (dto.AccountId is not null)
            {
                var account = await dbContext.Accounts
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(a => a.Id == dto.AccountId && a.UserId == userId, ct);

                if (account is null)
                    throw new NotFoundException("Account was not found");

                if (account.Status != AccountStatus.Active)
                    throw new ConflictException("Unable to use inactive account");
            }

            if (dto.CategoryId is not null &&
                !await dbContext.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId, ct))
            {
                throw new NotFoundException("Category was not found");
            }
        }
    }
}
