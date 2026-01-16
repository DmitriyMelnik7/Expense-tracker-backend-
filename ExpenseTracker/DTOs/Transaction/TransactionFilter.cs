using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Transaction
{
    public sealed class TransactionFilter
    {
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }

        public string? AccountId { get; init; }

        public string? CategoryId { get; init; }

        public bool? IsIncome { get; init; }

        public decimal? AmountFrom { get; init; }
        public decimal? AmountTo { get; init; }

        public string? Currency { get; init; }

        public string? Search { get; init; }
    }

    public sealed class TransactionFilterValidator : AbstractValidator<TransactionFilter>
    {
        public TransactionFilterValidator()
        {
            RuleFor(f => f.DateTo)
                .GreaterThanOrEqualTo(f => f.DateFrom)
                .When(f => f.DateFrom.HasValue && f.DateTo.HasValue);

            RuleFor(f => f.AmountTo)
                .GreaterThanOrEqualTo(f => f.AmountFrom)
                .When(f => f.AmountFrom.HasValue && f.AmountTo.HasValue);

            RuleFor(f => f.AmountFrom)
                .GreaterThanOrEqualTo(0)
                .When(f => f.AmountFrom.HasValue);

            RuleFor(f => f.AmountTo)
                .GreaterThanOrEqualTo(0)
                .When(f => f.AmountTo.HasValue);

            RuleFor(f => f.AccountId)
                .NotEmpty()
                .When(f => f.AccountId is not null);

            RuleFor(f => f.CategoryId)
                .NotEmpty()
                .When(f => f.CategoryId is not null);

            RuleFor(f => f.Currency)
                .Length(3)
                .When(f => !string.IsNullOrWhiteSpace(f.Currency));

            RuleFor(f => f.Search)
                .MaximumLength(100)
                .When(f => !string.IsNullOrWhiteSpace(f.Search));
        }
    }
}
