using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.DTOs.Auth
{
    public sealed class RegisterUserDto
    {
        public required string Email { get; set; }

        [MaxLength(20)]
        public required string DisplayName { get; set; }
        public required string Password { get; set; }
    }
}
