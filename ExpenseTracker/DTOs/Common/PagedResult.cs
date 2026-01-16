using Microsoft.Identity.Client;

namespace ExpenseTracker.Api.DTOs.Common
{
    public sealed class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

    }
}
