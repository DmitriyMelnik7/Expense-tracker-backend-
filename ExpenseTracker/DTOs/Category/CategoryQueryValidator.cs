using ExpenseTracker.Api.DTOs.Category.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Category
{
    public sealed class CategoryQueryValidator : AbstractValidator<QueryParameters<CategoryFilter, CategorySort>>
    {
        public CategoryQueryValidator(PaginationParamsValidator paginationValidator,
            CategoryFilterValidator filterValidator)
        {
            RuleFor(q => q.Pagination).SetValidator(paginationValidator);

            When(q => q.Filter is not null, () =>
            {
                RuleFor(q => q.Filter).SetValidator(filterValidator!);
            });
        }
    }
}
