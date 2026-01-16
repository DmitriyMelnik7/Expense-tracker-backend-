using ExpenseTracker.Api.DTOs.Budget;
using ExpenseTracker.Api.DTOs.Budget.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Entities;

namespace ExpenseTracker.Api.Extensions.QueryExtensions
{
    public static class BudgetQueryExtensions
    {
        public static IQueryable<Budget> ApplyFilter(
            this IQueryable<Budget> query,
            BudgetFilter filter)
        {
            if (filter is not null)
            {
                if (filter.Month.HasValue)
                    query = query.Where(b => b.Month == filter.Month);

                if (filter.Year.HasValue)
                    query = query.Where(b => b.Year == filter.Year);

                if (filter.MinAmount.HasValue)
                    query = query.Where(b => b.Amount >= filter.MinAmount);
            }

            return query;
        }

        public static IQueryable<Budget> ApplySort(
            this IQueryable<Budget> query,
            BudgetSort? sort)
        {
            if (sort is null)
                return query.OrderByDescending(b => b.Year)
                    .ThenByDescending(b => b.Month);

            return sort.Field switch
            {
                BudgetSortField.Date => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(b => b.Year).ThenBy(b => b.Month)
                    : query.OrderByDescending(b => b.Year).ThenByDescending(b => b.Month),

                BudgetSortField.Amount => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(b => b.Amount)
                    : query.OrderByDescending(b => b.Amount),

                _ => query.OrderByDescending(b => b.Year)
                    .ThenByDescending(b => b.Month)
            };
        }
    }
}
