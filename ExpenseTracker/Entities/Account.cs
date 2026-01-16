using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Entities
{
    public sealed class Account
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required Currency Currency { get; set; }
        public decimal Balance { get; set; }

        [Required]
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public AccountStatus Status { get; set; } = AccountStatus.Active;
        public DateTime? ClosedAt { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
