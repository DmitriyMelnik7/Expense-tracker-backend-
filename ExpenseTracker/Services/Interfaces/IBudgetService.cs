using ExpenseTracker.Api.DTOs.Budget;
using ExpenseTracker.Api.DTOs.Budget.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using Microsoft.AspNetCore.JsonPatch;

namespace ExpenseTracker.Api.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<PagedResult<BudgetDto>> GetAllAsync(QueryParameters<BudgetFilter, BudgetSort> queryParameters, string userId, CancellationToken ct);
        Task<BudgetDto> GetByIdAsync(string id, string userId, CancellationToken ct);
        Task<BudgetDto> CreateAsync(CreateBudgetDto dto, string userId, CancellationToken ct);
        Task<BudgetDto> UpdateAsync(string id, JsonPatchDocument<BudgetPatchDto> patch, string userId, CancellationToken ct);
        Task DeleteAsync(string id, string userId, CancellationToken ct);
    }
}
