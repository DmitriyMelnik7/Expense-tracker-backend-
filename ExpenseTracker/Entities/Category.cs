using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Entities
{
    public sealed class Category
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public required CategoryType CategoryType { get; set; }

        [Required]
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public string? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public List<Category> SubCategories { get; set; } = new List<Category>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
