using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;

namespace ShopEase.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<ProductResponse> CreateProductAsync(ProductRequest request);
        Task<IEnumerable<ProductResponse>> CreateMultipleProductsAsync(List<ProductRequest> requests); 
        Task<ProductResponse> UpdateProductAsync(int id, ProductRequest request);
        Task<bool> DeleteProductAsync(int id);
    }
}