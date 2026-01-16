using ExpenseTracker.Api.Database;
using ExpenseTracker.Api.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.DTOs.Category
{
    public sealed class CreateCategoryDto
    {
        public required string Name { get; set; }
        public required CategoryType CategoryType{ get; set; }
        public string? ParentCategoryId { get; set; }
    }

    public sealed class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator(ApplicationDbContext dbContext) 
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.CategoryType)
                .IsInEnum();
        }
    }
}
