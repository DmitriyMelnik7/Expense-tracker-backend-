using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Account.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Exceptions;
using ExpenseTracker.Api.Extensions;
using ExpenseTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("accounts")]
    [Authorize]
    public sealed class AccountController(IAccountService service) : ControllerBase
    {
        public string UserId => User.GetUserId() ?? throw new UnauthorizedException("Unauthorized"); //wont work
        public HashSet<string> allowedPatchPaths = 
            [
                "/name"
            ];

        [HttpGet]
        public async Task<ActionResult<PagedResult<AccountDto>>> GetAccounts(
            [FromQuery] QueryParameters<AccountFilter, AccountSort> queryParameters,
            CancellationToken ct)
        {
            var result = await service.GetAllAsync(queryParameters, UserId, ct);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetAccount(
            string id, 
            CancellationToken ct)
        {
            var account = await service.GetByIdAsync(id, UserId, ct);

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount(
            [FromBody] CreateAccountDto createAccountDto,
            CancellationToken ct)
        {
            var account = await service.CreateAsync(createAccountDto, UserId, ct);

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<AccountDto>> UpdateAccount(
            string id, 
            [FromBody] JsonPatchDocument<AccountPatchDto> patch, 
            CancellationToken ct)
        {
            foreach (var op in patch.Operations)
                if (!allowedPatchPaths.Contains(op.path))
                    return BadRequest($"Path \"{op.path}\" is not allowed");

            var account = await service.UpdateAsync(id, patch, UserId, ct);

            return Ok(account);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(
            string id, 
            CancellationToken ct)
        {
            await service.CloseAsync(id, UserId, ct);

            return NoContent();
        }
    }
}
