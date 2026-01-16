using ExpenseTracker.Api.DTOs.Common;

namespace ExpenseTracker.Api.DTOs.Transaction.Sorting
{
    public sealed class TransactionSort
    {
        public SortDirection Direction { get; init; } = SortDirection.Desc;
        public TransactionSortField Field { get; init; } = TransactionSortField.Date;
    }
}
