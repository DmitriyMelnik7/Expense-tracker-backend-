using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Account
{
    public sealed class AccountFilter
    {
        public string? Search { get; set; }
        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }
    }

    public sealed class AccountFilterValidator : AbstractValidator<AccountFilter>
    {
        public AccountFilterValidator()
        {
            RuleFor(a => a.Search)
                .MaximumLength(50)
                .When(a => a.Search is not null);

            RuleFor(a => a.MinBalance)
                .LessThanOrEqualTo(a => a.MaxBalance)
                .When(a => a.MinBalance.HasValue && a.MaxBalance.HasValue);
        }
    }
}
