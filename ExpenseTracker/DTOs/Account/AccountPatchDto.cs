using FluentValidation;

namespace ExpenseTracker.Api.DTOs.Account
{
    public sealed class AccountPatchDto
    {
        public string? Name { get; set; }
    }

    public sealed class AccountPatchDtoValidator : AbstractValidator<AccountPatchDto>
    {
        public AccountPatchDtoValidator() 
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(50)
                .When(a => a.Name is not null);
        }
    }
}
