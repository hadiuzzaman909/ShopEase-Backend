using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;

public interface IOrderService
{
    Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
    Task<OrderResponse> PlaceOrderAsync(string userId, OrderRequest request);
    Task<OrderResponse?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(string userId);
    Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
}