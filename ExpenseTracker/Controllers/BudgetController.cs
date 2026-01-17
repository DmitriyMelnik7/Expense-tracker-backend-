using ExpenseTracker.Api.DTOs.Budget;
using ExpenseTracker.Api.DTOs.Budget.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.Extensions;
using ExpenseTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("budgets")]
    [Authorize]
    public sealed class BudgetController(IBudgetService service) : ControllerBase
    {
        private string UserId => User.GetUserId() ?? throw new Exception("UserId not found");
        private HashSet<string> allowedPatchPaths =
        [
            "/amount",
            "/month",
            "/year"
        ];

        [HttpGet]
        public async Task<ActionResult<List<BudgetDto>>> GetBudgets(
            [FromQuery] QueryParameters<BudgetFilter, BudgetSort> queryParameters,
            CancellationToken ct)
        {
            var budgets = await service.GetAllAsync(queryParameters, UserId, ct);

            return Ok(budgets);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDto>> GetBudget(
            string id,
            CancellationToken ct)
        {
            var budget = await service.GetByIdAsync(id, UserId, ct);

            return Ok(budget);
        }

        [HttpPost]
        public async Task<ActionResult<BudgetDto>> CreateBudget(
            [FromBody] CreateBudgetDto createBudgetDto,
            CancellationToken ct)
        {
            var budget = await service.CreateAsync(createBudgetDto, UserId, ct);
;
            return CreatedAtAction(nameof(GetBudget),  new { id = budget.Id }, budget);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<BudgetDto>> UpdateBudget(
            string id, 
            [FromBody] JsonPatchDocument<BudgetPatchDto> patch, 
            CancellationToken ct)
        {
            foreach (var op in patch.Operations)
                if (!allowedPatchPaths.Contains(op.path))
                    return BadRequest($"Path \"{op.path}\" is not allowed");

            var budget = await service.UpdateAsync(id, patch, UserId, ct);

            return Ok(budget);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(
            string id,
            CancellationToken ct)
        {
            await service.DeleteAsync(id, UserId, ct);

            return NoContent();
        }
    }
}
