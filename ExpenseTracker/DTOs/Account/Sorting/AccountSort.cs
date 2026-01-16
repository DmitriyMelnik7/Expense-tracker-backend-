using ExpenseTracker.Api.DTOs.Common;

namespace ExpenseTracker.Api.DTOs.Account.Sorting
{
    public sealed class AccountSort
    {
        public SortDirection Direction { get; set; } = SortDirection.Desc;
        public AccountSortField Field { get; set; } = AccountSortField.Name;
    }
}
