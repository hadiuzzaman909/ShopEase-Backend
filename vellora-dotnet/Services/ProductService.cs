using AutoMapper;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Repositories.IRepositories;
using Vellora.ECommerce.API.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Vellora.ECommerce.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(
            int pageNumber, int pageSize, string sortBy, bool descending, string searchTerm, int? categoryId)
        {
            var query = _unitOfWork.Product.GetQueryable()
                .Include(p => p.Category)
                    .ThenInclude(c => c.SubCategories)
                .AsQueryable();

            // Searching
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            // Filtering by Category
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "name" => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                    "createdat" => descending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderBy(p => p.Id)
                };
            }

            // Pagination
            int totalRecords = await query.CountAsync();
            var products = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var mappedProducts = _mapper.Map<List<ProductResponse>>(products);

            return new PaginatedResponse<ProductResponse>
            {
                Data = mappedProducts,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }


        // Get a product by Id
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Product.GetQueryable()
                .Include(p => p.Category)
                    .ThenInclude(c => c.SubCategories)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) throw new KeyNotFoundException("Product not found");

            return _mapper.Map<ProductResponse>(product);
        }

        // Create a new product
        public async Task<ProductResponse> CreateProductAsync(ProductRequest request)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(request.CategoryId);
            if (category == null)
                throw new KeyNotFoundException("Category not found");

            var product = _mapper.Map<Product>(request);
            await _unitOfWork.Product.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        // Create multiple product
        public async Task<IEnumerable<ProductResponse>> CreateMultipleProductsAsync(List<ProductRequest> requests)
        {
            var products = _mapper.Map<List<Product>>(requests);

            await _unitOfWork.Product.AddRangeAsync(products);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<List<ProductResponse>>(products);
        }


        // Update an existing product
        public async Task<ProductResponse> UpdateProductAsync(int id, ProductRequest request)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");

            _mapper.Map(request, product);

            _unitOfWork.Product.Update(product);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        // Delete a product
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product == null) return false;

            _unitOfWork.Product.Remove(product);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
