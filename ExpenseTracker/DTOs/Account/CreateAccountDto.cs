using ExpenseTracker.Api.Entities;
using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Account
{
    public sealed class CreateAccountDto
    {
        public required string Name { get; set; }
        public required Currency Currency { get; set; }
        public decimal InitialBalance { get; set; } = 0;
    }

    public sealed class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator() 
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(a => a.Currency)
                .IsInEnum();

            RuleFor(a => a.InitialBalance).GreaterThanOrEqualTo(0);
        }
    }
}
