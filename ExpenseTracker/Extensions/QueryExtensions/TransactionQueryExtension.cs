using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.DTOs.Transaction;
using ExpenseTracker.Api.DTOs.Transaction.Sorting;
using ExpenseTracker.Api.Entities;

namespace ExpenseTracker.Api.Extensions.QueryExtensions
{
    public static class TransactionQueryExtension
    {
        public static IQueryable<Transaction> ApplyFilter(
            this IQueryable<Transaction> query,
            TransactionFilter filter)
        {
            if (filter is not null)
            {
                if (filter.DateFrom.HasValue)
                    query = query.Where(t => t.Date >= filter.DateFrom.Value);

                if (filter.DateTo.HasValue)
                    query = query.Where(t => t.Date <= filter.DateTo.Value);

                if (!string.IsNullOrEmpty(filter.AccountId))
                    query = query.Where(t => t.AccountId == filter.AccountId);

                if (!string.IsNullOrEmpty(filter.CategoryId))
                    query = query.Where(t => t.CategoryId == filter.CategoryId);

                if (filter.IsIncome.HasValue)
                    query = filter.IsIncome.Value
                        ? query.Where(t => t.Amount > 0)
                        : query.Where(t => t.Amount < 0);

                if (filter.AmountFrom.HasValue)
                    query = query.Where(t => t.Amount >= filter.AmountFrom.Value);

                if (filter.AmountTo.HasValue)
                    query = query.Where(t => t.Amount <= filter.AmountTo.Value);

                //if (!string.IsNullOrEmpty(filter.Currency))
                //    query = query.Where(t => t.Currency == filter.Currency);

                if (!string.IsNullOrEmpty(filter.Search))
                    query = query.Where(t =>
                        t.Description != null &&
                        t.Description.Contains(filter.Search));
            }

            return query;
        }

        public static IQueryable<Transaction> ApplySort(
            this IQueryable<Transaction> query,
            TransactionSort? sort)
        {
            if (sort is null)
                return query.OrderByDescending(t => t.Date);

            return sort.Field switch
            {
                TransactionSortField.Amount => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(t => t.Amount)
                    : query.OrderByDescending(t => t.Amount),
                TransactionSortField.Date => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(t => t.Date)
                    : query.OrderByDescending(t => t.Date),

                _ => query.OrderByDescending(t => t.Date)
            };
        }
    }
}
