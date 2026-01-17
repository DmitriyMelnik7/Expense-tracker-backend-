using ExpenseTracker.Api.Database;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.DTOs.Transaction
{
    public sealed class CreateTransactionDto
    {
        public required string AccountId { get; set; }
        public required string CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public string? ReceiptPath { get; set; }
    }

    public sealed class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
    {
        public CreateTransactionDtoValidator(ApplicationDbContext dbContext)
        {
            RuleFor(t => t.AccountId).NotEmpty();

            RuleFor(t => t.CategoryId).NotEmpty();

            RuleFor(t => t.Amount).GreaterThan(0);

            RuleFor(t => t.Date).LessThanOrEqualTo(_ => DateTime.UtcNow);

            RuleFor(t => t.Description).MaximumLength(250);
        }
    }
}
