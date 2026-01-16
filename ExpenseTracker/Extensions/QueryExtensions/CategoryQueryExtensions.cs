using ExpenseTracker.Api.DTOs.Category;
using ExpenseTracker.Api.DTOs.Category.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Entities;

namespace ExpenseTracker.Api.Extensions.QueryExtensions
{
    public static class CategoryQueryExtensions
    {
        public static IQueryable<Category> ApplyFilter(
            this IQueryable<Category> query,
            CategoryFilter filter)
        {
            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Search))
                    query = query.Where(c => c.Name.Contains(filter.Search));
            }

            return query;
        }

        public static IQueryable<Category> ApplySort(
            this IQueryable<Category> query,
            CategorySort? sort)
        {
            if(sort is null)
                return query.OrderByDescending(c => c.Name);

            return sort.Field switch
            {
                CategorySortField.Name => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name),

                CategorySortField.CategoryType => sort.Direction == SortDirection.Asc
                    ? query.OrderBy(c => c.CategoryType)
                    : query.OrderByDescending(c => c.CategoryType),

                _ => query.OrderByDescending(c => c.Name)
            };
        }
    }
}
