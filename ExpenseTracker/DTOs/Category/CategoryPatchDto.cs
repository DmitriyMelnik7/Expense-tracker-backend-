using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Category
{
    public sealed class CategoryPatchDto
    {
        public string? Name { get; set; }
        public string? ParentCategoryId { get; set; } 
    }

    public sealed class CategoryPatchDtoValidator : AbstractValidator<CategoryPatchDto>
    {
        public CategoryPatchDtoValidator()
        {
            
            RuleFor(c => c.Name)
                .MaximumLength(50)
                .NotEmpty()
                .When(c => c.Name is not null);

            RuleFor(c => c.ParentCategoryId)
                .MaximumLength(50)
                .NotEmpty()
                .When(c => c.ParentCategoryId is not null);
        }
    }
}
