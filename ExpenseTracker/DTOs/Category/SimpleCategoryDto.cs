namespace ExpenseTracker.Api.DTOs.Category
{
    public class SimpleCategoryDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string CategoryType { get; set; } = default!;
    }
}
