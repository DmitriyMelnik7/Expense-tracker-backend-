using ExpenseTracker.Api.DTOs.Common;

namespace ExpenseTracker.Api.DTOs.Budget.Sorting
{
    public sealed class BudgetSort
    {
        public SortDirection Direction { get; set; } = SortDirection.Desc;
        public BudgetSortField Field { get; set; } = BudgetSortField.Date;
    }
}
