using AutoMapper;
using ShopEase.Models;
using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;
using ShopEase.Repositories.IRepositories;
using ShopEase.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace ShopEase.Services
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

        // Get all products
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Product.GetQueryable()
                .Include(p => p.Category) 
                    .ThenInclude(c => c.SubCategories) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductResponse>>(products);
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
