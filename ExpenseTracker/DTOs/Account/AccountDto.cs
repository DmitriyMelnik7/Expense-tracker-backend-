using ExpenseTracker.Api.Entities;

namespace ExpenseTracker.Api.DTOs.Account
{
    public sealed class AccountDto
    {
        public required string Id { get; set; }
        public string Name { get; set; } = "";
        public required string Currency { get; set; }
        public decimal Balance { get; set; }
    }
}
