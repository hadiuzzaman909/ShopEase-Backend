using ShopEase.Models;

namespace ShopEase.Repositories.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}