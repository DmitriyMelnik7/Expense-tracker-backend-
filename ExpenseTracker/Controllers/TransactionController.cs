using Azure;
using ExpenseTracker.Api.Database;
using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.DTOs.Transaction;
using ExpenseTracker.Api.DTOs.Transaction.Sorting;
using ExpenseTracker.Api.Entities;
using ExpenseTracker.Api.Extensions;
using ExpenseTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("transactions")]
    [Authorize]
    public sealed class TransactionController(ITransactionService service) : ControllerBase
    {
        private string UserId => User.GetUserId() ?? throw new Exception("UserId not found");
        private HashSet<string> allowedPatchPaths =
        [
                "/amount",
                "/date",
                "/description",
                "/categoryId",
                "/accountId",
                "/receiptPath"
        ];

        [HttpGet]
        public async Task<ActionResult<PagedResult<TransactionDto>>> GetTransactions(
            [FromQuery] QueryParameters<TransactionFilter, TransactionSort> queryParameters,
            CancellationToken ct)
        {
            var transactions = await service.GetAllAsync(queryParameters, UserId, ct);

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(
            string id,
            CancellationToken ct)
        {
            var transaction = await service.GetByIdAsync(id, UserId, ct);

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> CreateTransaction(
            [FromBody] CreateTransactionDto createTransactionDto,
            CancellationToken ct)
        {
            var transaction = await service.CreateAsync(createTransactionDto, UserId, ct);

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TransactionDto>> UpdateTransaction(
            string id,
            [FromBody] JsonPatchDocument<TransactionPatchDto> patch,
            CancellationToken ct)
        {
            foreach (var op in patch.Operations)
                if (!allowedPatchPaths.Contains(op.path))
                    return BadRequest($"Path \"{op.path}\" is not allowed");

            var transaction = await service.UpdateAsync(id, patch, UserId, ct);

            return Ok(transaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(
            string id, 
            CancellationToken ct)
        {
            await service.DeleteAsync(id, UserId, ct);

            return NoContent();
        }
    }
}

