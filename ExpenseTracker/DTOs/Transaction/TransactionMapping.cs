using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Category;

namespace ExpenseTracker.Api.DTOs.Transaction
{
    public static  class TransactionMapping
    {
        public static TransactionDto ToDto(this ExpenseTracker.Api.Entities.Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Category = new SimpleCategoryDto
                {
                    Id = transaction.Category.Id,
                    Name = transaction.Category.Name,
                    CategoryType = transaction.Category.CategoryType.ToString()
                },
                Account = new SimpleAccountDto
                {
                    Id = transaction.Account.Id,
                    Name = transaction.Account.Name
                }
            };
        }

        public static ExpenseTracker.Api.Entities.Transaction ToEntity(this CreateTransactionDto createTransactionDto, string userId)
        {
            return new Entities.Transaction
            {
                Id = $"t_{Guid.CreateVersion7().ToString()}",
                UserId = userId,
                AccountId = createTransactionDto.AccountId,
                CategoryId = createTransactionDto.CategoryId,
                Amount = createTransactionDto.Amount,
                Date = createTransactionDto.Date,
                Description = createTransactionDto.Description,
                ReceiptPath = createTransactionDto.ReceiptPath,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static void UpdateFromDto(this Entities.Transaction transaction, TransactionPatchDto updateTransactionDto)
        {
            if(updateTransactionDto.AccountId is not null)
                transaction.AccountId = updateTransactionDto.AccountId;

            if (updateTransactionDto.CategoryId is not null)
                transaction.CategoryId = updateTransactionDto.CategoryId;

            //if (updateTransactionDto.Currency is not null)
            //    transaction.Currency = updateTransactionDto.Currency;

            if (updateTransactionDto.Amount.HasValue)
                transaction.Amount = updateTransactionDto.Amount.Value;

            if (updateTransactionDto.Date.HasValue)
                transaction.Date = updateTransactionDto.Date.Value;

            if (updateTransactionDto.Description is not null)
                transaction.Description = updateTransactionDto.Description;

            if (updateTransactionDto.ReceiptPath is not null)
                transaction.ReceiptPath = updateTransactionDto.ReceiptPath;
        }
    }
}
