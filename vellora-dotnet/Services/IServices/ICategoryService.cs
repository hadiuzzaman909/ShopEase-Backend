using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;

namespace Vellora.ECommerce.API.Services.IServices
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
