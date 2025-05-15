using Vellora.ECommerce.API.Models;
namespace Vellora.ECommerce.API.DTOs.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
        public decimal TotalAmount => OrderItems.Sum(oi => oi.TotalPrice);
    }

    public class OrderItemResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}