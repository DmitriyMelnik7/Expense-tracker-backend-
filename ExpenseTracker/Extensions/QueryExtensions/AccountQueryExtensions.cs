using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Account.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Entities;

namespace ExpenseTracker.Api.Extensions.QueryExtensions
{
    public static class AccountQueryExtensions
    {
        public static IQueryable<Account> ApplyFilter(
            this IQueryable<Account> query,
            AccountFilter filter)
        {
            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Search))
                    query = query.Where(a => a.Name.Contains(filter.Search));

                if (filter.MinBalance.HasValue)
                    query = query.Where(a => a.Balance >= filter.MinBalance.Value);

                if (filter.MaxBalance.HasValue)
                    query = query.Where(a => a.Balance <= filter.MaxBalance.Value);
            }

            return query;
        }

        public static IQueryable<Account> ApplySort(
            this IQueryable<Account> query,
            AccountSort? sort)
        {
            if (sort is null)
                return query.OrderByDescending(a => a.Name);

            return sort.Field switch
            {
                AccountSortField.Name => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(a => a.Name)
                    : query.OrderByDescending(a => a.Name),

                AccountSortField.Balance => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(a => a.Balance)
                    : query.OrderByDescending(a => a.Balance),

                _ => query.OrderByDescending(a => a.Name)
            };
        }
    }
}
