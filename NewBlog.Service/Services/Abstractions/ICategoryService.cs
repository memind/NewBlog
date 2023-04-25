using NewBlog.Entity.DTOs.Categories;
using NewBlog.Entity.Entities;

namespace NewBlog.Service.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesNonDeleted();
        Task<List<CategoryDto>> GetAllCategoriesDeleted();
        Task<List<CategoryDto>> GetAllCategoriesDeletedTake24();
        Task CreateCategoryAsync(CategoryAddDto model);
        Task<Category> GetCategoryByGuid(Guid id);
        Task<string> UpdateCategoryAsync(CategoryUpdateDto model);
        Task<string> SafeDeleteCategoryAsync(Guid categoryId);
        Task<string> UndoDeleteCategoryAsync(Guid categoryId);
    }
}
