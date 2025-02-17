using Microsoft.AspNetCore.Mvc;
using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;
using ShopEase.Services.IServices;

namespace ShopEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ✅ Get all products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // ✅ Get product by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // ✅ Create a new product
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest productRequest)
        {
            try
            {
                var product = await _productService.CreateProductAsync(productRequest);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Create multiple products (Bulk Insertion)
        [HttpPost("bulk")]
        public async Task<IActionResult> CreateMultipleProducts([FromBody] List<ProductRequest> productRequests)
        {
            if (productRequests == null || productRequests.Count == 0)
            {
                return BadRequest("Product list cannot be empty.");
            }

            try
            {
                var products = await _productService.CreateMultipleProductsAsync(productRequests);
                return Ok(products);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Update an existing product
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest productRequest)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, productRequest);
                return Ok(updatedProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // ✅ Delete a product
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            bool deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound("Product not found.");

            return Ok("Product deleted.");
        }
    }
}
