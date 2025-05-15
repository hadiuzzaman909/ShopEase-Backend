using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;

namespace Vellora.ECommerce.API.Services.IServices
{
    public interface ICartService
    {
        Task<CartResponse> GetCartByUserIdAsync(string userId);
        Task<CartResponse> AddItemToCartAsync(string userId, CartItemRequest request);
        Task<CartResponse> UpdateCartItemAsync(string userId, int cartItemId, CartItemUpdateRequest request);
        Task<bool> RemoveCartItemAsync(string userId, int cartItemId);
        Task<bool> ClearCartAsync(string userId);
    }
}
