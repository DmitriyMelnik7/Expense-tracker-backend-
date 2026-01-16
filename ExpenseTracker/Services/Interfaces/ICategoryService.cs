using ExpenseTracker.Api.DTOs.Category;
using ExpenseTracker.Api.DTOs.Category.Sorting;
using ExpenseTracker.Api.DTOs.Common;
using Microsoft.AspNetCore.JsonPatch;

namespace ExpenseTracker.Api.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResult<CategoryDto>> GetAllAsync(QueryParameters<CategoryFilter, CategorySort> queryParameters, string userId, CancellationToken ct);
        Task<CategoryDto> GetByIdAsync(string id, string userId, CancellationToken ct);
        Task<CategoryDto> CreateAsync (CreateCategoryDto dto, string userId, CancellationToken ct);
        Task<CategoryDto> UpdateAsync (string id, JsonPatchDocument<CategoryPatchDto> patch, string userId, CancellationToken ct);
        Task DeleteAsync(string id, string userId, CancellationToken ct);
    }
}
