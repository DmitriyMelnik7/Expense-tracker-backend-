using ExpenseTracker.Api.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.DTOs.Budget
{
    public sealed class CreateBudgetDto
    {
        public required string CategoryId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
    }

    public sealed class CreateBudgetDtoValidator : AbstractValidator<CreateBudgetDto>
    {
        public CreateBudgetDtoValidator(ApplicationDbContext dbContext)
        {
            RuleFor(b => b.CategoryId).NotEmpty();

            RuleFor(b => b.Month).InclusiveBetween(1, 12);

            RuleFor(b => b.Year)
                    .Must(year =>
                    {
                        var currentYear = DateTime.UtcNow.Year;
                        return year >= currentYear - 1 && year <= currentYear + 5;
                    });

            RuleFor(b => b.Amount).GreaterThanOrEqualTo(0);

            //RuleFor(b => b)
            //    .MustAsync(async (dto, cancellation) =>
            //    !await dbContext.Budgets.AnyAsync(b =>
            //        b.CategoryId == dto.CategoryId &&
            //        b.Month == dto.Month &&
            //        b.Year == dto.Year))
            //    .WithMessage("Budget for this category and period already exists");
        }
    }
}
