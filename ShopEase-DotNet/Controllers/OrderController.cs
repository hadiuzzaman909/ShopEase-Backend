using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;
using ShopEase.Models;
using System.Security.Claims;

namespace ShopEase.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("all-orders")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // Place an order (Requires authentication)
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized("User not found.");

            var order = await _orderService.PlaceOrderAsync(userId, request);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
        }

        // Get order by ID
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound("Order not found.");
            return Ok(order);
        }

        // Get all orders for the authenticated user
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetOrdersByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized("User not found.");

            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // Update order status (Admin Only)
        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] OrderStatusUpdateRequest request)
        {
        if (request == null)
            return BadRequest("Request body is missing.");

        if (!Enum.IsDefined(typeof(OrderStatus), request.Status))
            return BadRequest("Invalid order status.");

        var result = await _orderService.UpdateOrderStatusAsync(orderId, request.Status);
        if (!result) return NotFound("Order not found.");

        return Ok(new { Message = "Order status updated successfully." });
        }
    }
}