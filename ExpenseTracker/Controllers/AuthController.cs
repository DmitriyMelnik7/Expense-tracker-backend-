using ExpenseTracker.Api.DTOs.Auth;
using ExpenseTracker.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ExpenseTracker.Api.Controllers
{
    //!СДЕЛАТЬ СЕРВИС
    [ApiController]
    [Route("auth")]
    public class AuthController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = new ApplicationUser { UserName = registerUserDto.Email, Email = registerUserDto.Email, DisplayName = registerUserDto.DisplayName };
            var result = await userManager.CreateAsync(user, registerUserDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            await userManager.AddToRoleAsync(user, "User");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) 
                return Unauthorized();

            var result = await signInManager.PasswordSignInAsync(user, dto.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded) 
                return Unauthorized();

            // Можно вернуть профиль или просто Ok
            return Ok(new { user.Email, user.UserName });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
    }
}
