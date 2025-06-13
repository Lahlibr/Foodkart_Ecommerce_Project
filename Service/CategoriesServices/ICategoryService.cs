using Foodkart.DTOs.ViewDto;

namespace Foodkart.Service.CategoriesServices
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(CategoryViewDto categoryDto,IFormFile image);
        Task<List<CategoryViewDto>> GetAllCategories();
        Task<CategoryViewDto> GetCategoryById(int id);
        Task<bool> UpdateCategory(int id, CategoryViewDto categoryDto,IFormFile image);
        Task<bool> DeleteCategory(int id);
    }
}
