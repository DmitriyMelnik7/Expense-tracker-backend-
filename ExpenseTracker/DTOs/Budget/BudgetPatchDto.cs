using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Budget
{
    public sealed class BudgetPatchDto
    {
        public decimal? Amount { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

    public sealed class BudgetPatchDtoValidator : AbstractValidator<BudgetPatchDto>
    {
        public BudgetPatchDtoValidator()
        {
            RuleFor(b => b.Amount)
                .GreaterThanOrEqualTo(0)
                .When(b => b.Amount.HasValue);

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
        }
    }
}
