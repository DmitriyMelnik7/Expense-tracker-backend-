using ExpenseTracker.Api.DTOs.Account.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Query;

namespace ExpenseTracker.Api.DTOs.Account
{
    public sealed class AccountQueryValidator : AbstractValidator<QueryParameters<AccountFilter, AccountSort>>
    {
        public AccountQueryValidator(
            PaginationParamsValidator paginationValidator,
            AccountFilterValidator filterValidator)
        {
            RuleFor(q => q.Pagination).SetValidator(paginationValidator);

            When(q => q.Filter is not null, () =>
            {
                RuleFor(q => q.Filter).SetValidator(filterValidator!);
            });
        }
    }
}
