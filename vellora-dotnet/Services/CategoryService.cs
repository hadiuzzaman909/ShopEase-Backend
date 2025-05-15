using Vellora.ECommerce.API.Repositories.IRepositories;
using Vellora.ECommerce.API.Services.IServices;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Vellora.ECommerce.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Category
                .GetQueryable()  
                .Include(c => c.SubCategories)  
                .ToListAsync();

            var categoryResponses = categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                SubCategories = c.SubCategories.Select(sc => new SubCategoryResponse
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Description = sc.Description
                }).ToList()
            });

            return categoryResponses;
        }


        public async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Category
                .GetQueryable()
                .Include(c => c.SubCategories)  
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) throw new KeyNotFoundException("Category not found");

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SubCategories = category.SubCategories.Select(sc => new SubCategoryResponse
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Description = sc.Description
                }).ToList()
            };
        }


        // Add Category with subcategories
        public async Task<CategoryResponse> AddCategoryAsync(CategoryRequest categoryRequest)
        {
            var category = new Category
            {
                Name = categoryRequest.Name,
                Description = categoryRequest.Description
            };


            await _unitOfWork.Category.AddAsync(category);
            await _unitOfWork.SaveAsync();  

            if (categoryRequest.SubCategories != null && categoryRequest.SubCategories.Any())
            {
                foreach (var subCategoryRequest in categoryRequest.SubCategories)
                {
                    var subCategory = new Category
                    {
                        Name = subCategoryRequest.Name,
                        Description = subCategoryRequest.Description,
                        ParentCategoryId = category.Id 
                    };
                    category.SubCategories.Add(subCategory);
                }

                // Save subcategories
                await _unitOfWork.SaveAsync();
            }

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SubCategories = category.SubCategories.Select(sc => new SubCategoryResponse
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Description = sc.Description
                }).ToList()
            };
        }


        // Update an existing category
        public async Task<CategoryResponse> UpdateCategoryAsync(int id, CategoryRequest categoryRequest)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("Category not found");

            category.Name = categoryRequest.Name;
            category.Description = categoryRequest.Description;

            // Handle subcategories if provided
            if (categoryRequest.SubCategories != null && categoryRequest.SubCategories.Any())
            {
                // Delete existing subcategories if any
                var existingSubCategories = category.SubCategories.ToList();
                foreach (var subCategory in existingSubCategories)
                {
                    category.SubCategories.Remove(subCategory);
                }

                // Add new subcategories
                foreach (var subCategoryRequest in categoryRequest.SubCategories)
                {
                    var subCategory = new Category
                    {
                        Name = subCategoryRequest.Name,
                        Description = subCategoryRequest.Description,
                        ParentCategoryId = category.Id
                    };
                    category.SubCategories.Add(subCategory);
                }
            }

            _unitOfWork.Category.Update(category);
            await _unitOfWork.SaveAsync();

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SubCategories = category.SubCategories.Select(sc => new SubCategoryResponse
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Description = sc.Description
                }).ToList()
            };
        }

        // Delete a category if there has no subCategories
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category == null) return false;

            // Delete all subcategories before removing the category
            foreach (var subCategory in category.SubCategories.ToList())
            {
                _unitOfWork.Category.Remove(subCategory);
            }

            _unitOfWork.Category.Remove(category);
            await _unitOfWork.SaveAsync();

            return true;
        }

    }
}