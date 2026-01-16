using ExpenseTracker.Api.Database;
using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Transaction
{
    public sealed class TransactionPatchDto
    {
        public string? AccountId { get; set; }
        public string? CategoryId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public string? ReceiptPath { get; set; }
    }
    public sealed class TransactionPatchDtoValidator : AbstractValidator<TransactionPatchDto>
    {
        public TransactionPatchDtoValidator(ApplicationDbContext dbContext)
        {
            RuleFor(t => t.AccountId)
                .NotEmpty()
                .When(t => t.AccountId is not null);

            RuleFor(t => t.CategoryId)
                .NotEmpty()
                .When(t => t.CategoryId is not null);

            RuleFor(t => t.Amount)
                .GreaterThan(0)
                .When(t => t.Amount.HasValue);

            RuleFor(t => t.Date)
                .LessThanOrEqualTo(_ => DateTime.UtcNow)
                .When(t => t.Date.HasValue);

            RuleFor(t => t.Description)
                .MaximumLength(250)
                .When(t => t.Description is not null);
        }
    }
}
