using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.DTOs.Transaction.Sorting;
using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Transaction
{
    public sealed class TransactionQueryValidator : AbstractValidator<QueryParameters<TransactionFilter, TransactionSort>>
    {
        public TransactionQueryValidator(
            PaginationParamsValidator paginationValidator,
            TransactionFilterValidator filterValidator)
        {
            RuleFor(q => q.Pagination).SetValidator(paginationValidator);

            When(q => q.Filter is not null, () =>
            {
                RuleFor(q => q.Filter).SetValidator(filterValidator!);
            });
        }
    }
}
