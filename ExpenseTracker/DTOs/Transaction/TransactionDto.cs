using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Category;
using ExpenseTracker.Api.DTOs.User;

namespace ExpenseTracker.Api.DTOs.Transaction
{
    public sealed class TransactionDto
    {
        public string Id { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = default!;
        public DateTime Date { get; set; }

        public SimpleCategoryDto Category { get; set; } = default!;
        public SimpleAccountDto Account { get; set; } = default!;
    }
}
