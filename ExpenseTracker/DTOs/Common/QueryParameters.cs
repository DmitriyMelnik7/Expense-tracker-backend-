namespace ExpenseTracker.Api.DTOs.Common
{
    public sealed class QueryParameters<TFilter, TSort>
    {
        public PaginationParams Pagination { get; set; } = new();
        public TFilter? Filter { get; set; }
        public TSort? Sort { get; set; }
    }
}
