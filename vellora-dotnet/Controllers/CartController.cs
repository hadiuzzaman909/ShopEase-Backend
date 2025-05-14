using Microsoft.AspNetCore.Mvc;
using ShopEase.DTOs.Request;
using ShopEase.Services.IServices;
using System.Security.Claims;

namespace ShopEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Get Cart for the Logged-in User
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not found");

            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        // Add Item to Cart
        [HttpPost("add")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not found");

            var cart = await _cartService.AddItemToCartAsync(userId, request);
            return Ok(cart);
        }

        // Update Item Quantity in Cart
        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] CartItemUpdateRequest request)
        {
            if (request.Quantity <= 0)
                return BadRequest("Quantity must be greater than zero.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var updatedCart = await _cartService.UpdateCartItemAsync(userId, cartItemId, request);
            return Ok(updatedCart);
        }


        // Remove Item from Cart
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not found");

            var success = await _cartService.RemoveCartItemAsync(userId, cartItemId);
            if (!success) return NotFound("Cart item not found.");

            return Ok("Cart item removed successfully.");
        }

        // Clear Entire Cart
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("User not found");

            var success = await _cartService.ClearCartAsync(userId);
            if (!success) return BadRequest("Failed to clear cart.");

            return Ok("Cart cleared successfully.");
        }
    }
}