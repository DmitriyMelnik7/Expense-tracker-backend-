using ExpenseTracker.Api.DTOs.Category;

namespace ExpenseTracker.Api.DTOs.Category
{
    public static class CategoryMappings
    {
        public static Entities.Category ToEntity(this CreateCategoryDto createCategoryDto, string userId)
        {
            Entities.Category category = new Entities.Category
            {
                Id = $"c_{Guid.CreateVersion7()}",
                Name = createCategoryDto.Name,
                UserId = userId,
                CategoryType = createCategoryDto.CategoryType,
                ParentCategoryId = createCategoryDto.ParentCategoryId
            };

            return category;
        }

        public static CategoryDto ToDto(this Entities.Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CategoryType = category.CategoryType.ToString(),
                ParentCategory = category.ParentCategory?.Name ?? ""
            };
        }

        public static void UpdateFromDto(this Entities.Category category, CategoryPatchDto updateCategoryDto)
        {
            if(updateCategoryDto.Name is not null)
                category.Name = updateCategoryDto.Name;
            
            if(updateCategoryDto.ParentCategoryId is not null)
                category.ParentCategoryId = updateCategoryDto.ParentCategoryId;
        }
    }
}