using AutoMapper;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Order.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        // Place a new order
        public async Task<OrderResponse> PlaceOrderAsync(string userId, OrderRequest request)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty.");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod,
                Status = OrderStatus.Pending,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price,
                    TotalPrice = ci.Quantity * ci.Product.Price
                }).ToList()
            };

            await _unitOfWork.Order.AddAsync(order);
            _unitOfWork.Cart.Remove(cart); // Clear cart after order placement
            await _unitOfWork.SaveAsync();

            return _mapper.Map<OrderResponse>(order);
        }

        // Get order by ID
        public async Task<OrderResponse?> GetOrderByIdAsync(int orderId)
        {
            var order = await _unitOfWork.Order.GetOrderByIdAsync(orderId);
            return order != null ? _mapper.Map<OrderResponse>(order) : null;
        }

        // Get all orders for a specific user
        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _unitOfWork.Order.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        // Update order status
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var updated = await _unitOfWork.Order.UpdateOrderStatusAsync(orderId, status);
            if (!updated) return false;

            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}