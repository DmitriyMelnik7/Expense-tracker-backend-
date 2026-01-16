namespace ExpenseTracker.Api.DTOs.Category
{
    public sealed class CategoryDto
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? CategoryType { get; set; }
        public string? ParentCategory { get; set; }
    }
}
