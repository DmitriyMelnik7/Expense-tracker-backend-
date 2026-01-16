using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Category
{
    public sealed class CategoryFilter
    {
        public string? Search { get; set; }
    }

    public sealed class CategoryFilterValidator : AbstractValidator<CategoryFilter>
    {
        public CategoryFilterValidator()
        {
            RuleFor(c => c.Search)
                .MaximumLength(50)
                .When(c => c.Search is not null);
        }
    }
}
