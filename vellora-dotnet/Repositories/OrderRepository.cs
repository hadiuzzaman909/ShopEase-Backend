﻿
using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        // Get order by ID (including order items and product details)
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        // Get all orders for a specific user
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        // Update order status
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            return true;
        }
    }
}