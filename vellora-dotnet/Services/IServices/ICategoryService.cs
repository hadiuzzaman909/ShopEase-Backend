using ShopEase.Models;
using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;

namespace ShopEase.Services.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync();
        Task<CategoryResponse> GetCategoryByIdAsync(int id);
        Task<CategoryResponse> AddCategoryAsync(CategoryRequest categoryRequest);
        Task<CategoryResponse> UpdateCategoryAsync(int id, CategoryRequest categoryRequest);
        Task<bool> DeleteCategoryAsync(int id);

    }
}
