using ExpenseTracker.Api.DTOs.Common;
using ExpenseTracker.Api.DTOs.Transaction;
using ExpenseTracker.Api.DTOs.Transaction.Sorting;
using Microsoft.AspNetCore.JsonPatch;

namespace ExpenseTracker.Api.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedResult<TransactionDto>> GetAllAsync(QueryParameters<TransactionFilter, TransactionSort> queryParameters, string userId, CancellationToken ct);
        Task<TransactionDto> GetByIdAsync(string id, string userId, CancellationToken ct);
        Task<TransactionDto> CreateAsync(CreateTransactionDto dto, string userId, CancellationToken ct);
        Task<TransactionDto> UpdateAsync(string id, JsonPatchDocument<TransactionPatchDto> dto, string userId, CancellationToken ct);
        Task DeleteAsync(string id, string userId, CancellationToken ct);
    }
}
