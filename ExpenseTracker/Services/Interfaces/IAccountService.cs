using ExpenseTracker.Api.DTOs.Account;
using ExpenseTracker.Api.DTOs.Account.Sorting;
using ExpenseTracker.Api.DTOs.Category;
using ExpenseTracker.Api.DTOs.Common;
using Microsoft.AspNetCore.JsonPatch;

namespace ExpenseTracker.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<PagedResult<AccountDto>> GetAllAsync(QueryParameters<AccountFilter, AccountSort> queryParameters, string userId, CancellationToken ct);
        Task<AccountDto> GetByIdAsync(string id, string userId, CancellationToken ct);
        Task<AccountDto> CreateAsync(CreateAccountDto dto, string userId, CancellationToken ct);
        Task<AccountDto> UpdateAsync(string id, JsonPatchDocument<AccountPatchDto> patch, string userId, CancellationToken ct);
        Task CloseAsync(string id, string userId, CancellationToken ct);
    }
}
