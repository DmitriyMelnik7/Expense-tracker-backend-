using Azure;
using ExpenseTracker.Api.DTOs.Category;
using ExpenseTracker.Api.DTOs.Category.Sorting;
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
    [Route("categories")]
    [Authorize]
    public sealed class CategoriesController(ICategoryService service) : ControllerBase
    {
        private string UserId => User.GetUserId() ?? throw new Exception("UserId not found");
        private HashSet<string> allowedPatchPaths =
        [
            "/parentCategoryId",
            "/name"
        ];

        [HttpGet]
        public async Task<ActionResult<PagedResult<CategoryDto>>> GetCategories(
            [FromQuery] QueryParameters<CategoryFilter, CategorySort> queryParameters,
            CancellationToken ct)
        {
            var result = await service.GetAllAsync(queryParameters, UserId, ct);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(
            string id, 
            CancellationToken ct)
        {
            var category = await service.GetByIdAsync(id, UserId, ct);

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(
            CreateCategoryDto createCategoryDto, 
            CancellationToken ct)
        {
            var category = await service.CreateAsync(createCategoryDto, UserId, ct);

            return CreatedAtAction(nameof (GetCategory), new { id = category.Id }, category);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(
            string id, 
            [FromBody] JsonPatchDocument<CategoryPatchDto> patch,
            CancellationToken ct)
        {
            foreach (var op in patch.Operations)
                if (!allowedPatchPaths.Contains(op.path))
                    return BadRequest($"Path \"{op.path}\" is not allowed");

            var category = await service.UpdateAsync(id, patch, UserId, ct);

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(
            string id, 
            CancellationToken ct)
        {
            await service.DeleteAsync(id, UserId, ct);

            return NoContent();
        }
    }
}
