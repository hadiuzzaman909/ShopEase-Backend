using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;

namespace ShopEase.Services.IServices
{
    public interface ICartService
    {
        Task<CartResponse> GetCartByUserIdAsync(string userId);
        Task<CartResponse> AddItemToCartAsync(string userId, CartItemRequest request);
        Task<CartResponse> UpdateCartItemAsync(string userId, int cartItemId, int quantity);
        Task<bool> RemoveCartItemAsync(string userId, int cartItemId);
        Task<bool> ClearCartAsync(string userId);
    }
}
