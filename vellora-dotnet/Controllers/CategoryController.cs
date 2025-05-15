using Microsoft.AspNetCore.Mvc;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Services.IServices;

namespace Vellora.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Get all categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // Get category by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Create a new category
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                var category = await _categoryService.AddCategoryAsync(categoryRequest);
                return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update an existing category
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
                return Ok(updatedCategory);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        // Delete a category
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool deleted = await _categoryService.DeleteCategoryAsync(id);
            if (!deleted)
                return NotFound("Category not found.");

            return Ok("Category deleted.");
        }


        // Insert multiple categories with subcategories
        [HttpPost("bulk")]
        public async Task<IActionResult> AddMultipleCategories([FromBody] List<CategoryRequest> categoriesRequest)
        {
            try
            {
                var categories = new List<CategoryResponse>();

                foreach (var categoryRequest in categoriesRequest)
                {
                    var category = await _categoryService.AddCategoryAsync(categoryRequest);
                    categories.Add(category);
                }

                return CreatedAtAction(nameof(GetAllCategories), categories);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}