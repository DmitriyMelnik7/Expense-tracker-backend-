namespace ExpenseTracker.Api.DTOs.Budget
{
    public sealed class BudgetDto
    {
        public required string Id { get; set; }
        public required string CategoryName { get; set; }
        public int Month { get; set; }
        public int Year  { get; set; }
        public decimal Amount { get; set; }
    }
}
