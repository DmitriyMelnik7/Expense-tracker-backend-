using ExpenseTracker.Api.DTOs.Common;

namespace ExpenseTracker.Api.DTOs.Category.Sorting
{
    public sealed class CategorySort
    {
        public SortDirection Direction { get; set; } = SortDirection.Desc;
        public CategorySortField Field { get; set; } = CategorySortField.Name;
    }
}
