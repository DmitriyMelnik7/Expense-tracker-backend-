using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Entities
{
    public sealed class Budget
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        [Required]
        public required string CategoryId { get; set; }
        public Category Category { get; set; } = default!;

        public int Month { get; set; } = DateTime.UtcNow.Month;
        public int Year { get; set; } = DateTime.UtcNow.Year;
        
        public decimal Amount { get; set; }
    }
}
