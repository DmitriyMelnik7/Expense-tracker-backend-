using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Budget
{
    public sealed class BudgetFilter
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? MinAmount { get; set; }
    }

    public sealed class BudgetFilterValidator : AbstractValidator<BudgetFilter>
    {
        public BudgetFilterValidator()
        {
            RuleFor(b => b.Month)
                .InclusiveBetween(1, 12)
                .When(b => b.Month.HasValue);

            RuleFor(b => b.Year)
                .Must(year =>
                {
                    var currentYear = DateTime.UtcNow.Year;
                    return year >= currentYear - 1 && year <= currentYear + 5;
                })
                .When(b => b.Year.HasValue);

            RuleFor(b => b.MinAmount)
                .GreaterThan(0)
                .When(b => b.MinAmount.HasValue);
        }
    }
}
