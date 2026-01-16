using System.Runtime.CompilerServices;

namespace ExpenseTracker.Api.DTOs.Budget
{
    public static class BudgetMapping
    {
        public static Entities.Budget ToEntity(this CreateBudgetDto createBudgetDto, string userId)
        {
            return new Entities.Budget
            {
                Id = $"b_{Guid.CreateVersion7()}",
                UserId = userId,
                CategoryId = createBudgetDto.CategoryId,
                Month = createBudgetDto.Month,
                Year = createBudgetDto.Year,
                Amount = createBudgetDto.Amount
            };
        }

        public static void UpdateFromDto(this Entities.Budget budget, BudgetPatchDto updateBudgetDto)
        {
            if(updateBudgetDto.Amount.HasValue)
                budget.Amount = updateBudgetDto.Amount.Value;

            if (updateBudgetDto.Year.HasValue)
                budget.Year = updateBudgetDto.Year.Value;

            if (updateBudgetDto.Month.HasValue)
                budget.Month = updateBudgetDto.Month.Value;
        }

        public static BudgetDto ToDto(this Entities.Budget budget)
        {
            return new BudgetDto
            {
                Id = budget.Id,
                CategoryName = budget.Category.Name,
                Month = budget.Month,
                Year = budget.Year,
                Amount = budget.Amount
            };
        }
    }
}
