using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Entities
{
    public sealed class Transaction
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        [Required]
        public required string AccountId {  get; set; }
        public Account Account { get; set; } = default!;

        [Required]
        public required string CategoryId { get; set; }
        public Category Category { get; set; } = default!;
         
        public decimal Amount { get; set; }    

        [Required]
        //public required Currency Currency { get; set; }
        public DateTime Date {  get; set; }
        public string? Description { get; set; }

        public string? ReceiptPath { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

    }
}
