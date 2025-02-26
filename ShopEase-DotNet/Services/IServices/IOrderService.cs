using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;
using ShopEase.Models;

public interface IOrderService
{
    Task<OrderResponse> PlaceOrderAsync(string userId, OrderRequest request);
    Task<OrderResponse?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(string userId);
    Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
}