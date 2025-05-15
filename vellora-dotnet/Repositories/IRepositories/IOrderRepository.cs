using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;

namespace Vellora.ECommerce.API.Repositories.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int orderId);

        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);

    }
}