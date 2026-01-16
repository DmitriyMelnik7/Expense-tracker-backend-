using ExpenseTracker.Api.DTOs.Budget.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Budget
{
    public sealed class BudgetQueryValidator : AbstractValidator<QueryParameters<BudgetFilter, BudgetSort>>
    {
        public BudgetQueryValidator(
            PaginationParamsValidator paginationValidator,
            BudgetFilterValidator filterValidator)
        {
            RuleFor(q => q.Pagination).SetValidator(paginationValidator);

            When(q => q.Filter is not null, () =>
            {
                RuleFor(q => q.Filter).SetValidator(filterValidator!);
            });
        }
    }
}
