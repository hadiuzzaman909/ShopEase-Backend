using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;

namespace ShopEase.Services.IServices
{
    public interface IProductService
    {
        /// <summary>
        /// Get all products with pagination, sorting, searching, and filtering.
        /// </summary>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">Number of products per page.</param>
        /// <param name="sortBy">Field to sort by (e.g., "price", "name", "createdAt").</param>
        /// <param name="descending">Sort order (true for descending, false for ascending).</param>
        /// <param name="searchTerm">Search term to filter products by name or description.</param>
        /// <param name="categoryId">Optional category ID to filter products.</param>
        /// <returns>Paginated list of products.</returns>
        Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(int pageNumber, int pageSize, string sortBy, bool descending, string searchTerm, int? categoryId);

        /// <summary>
        /// Get a single product by ID.
        /// </summary>
        Task<ProductResponse> GetProductByIdAsync(int id);

        /// <summary>
        /// Create a new product.
        /// </summary>
        Task<ProductResponse> CreateProductAsync(ProductRequest request);

        /// <summary>
        /// Create multiple products in bulk.
        /// </summary>
        Task<IEnumerable<ProductResponse>> CreateMultipleProductsAsync(List<ProductRequest> requests);

        /// <summary>
        /// Update an existing product.
        /// </summary>
        Task<ProductResponse> UpdateProductAsync(int id, ProductRequest request);

        /// <summary>
        /// Delete a product by ID.
        /// </summary>
        Task<bool> DeleteProductAsync(int id);
    }
}
