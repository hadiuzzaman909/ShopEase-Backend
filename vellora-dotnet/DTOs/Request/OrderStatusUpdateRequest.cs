using Vellora.ECommerce.API.Models;
namespace Vellora.ECommerce.API.DTOs.Request
{
    public class OrderStatusUpdateRequest
    {
        public OrderStatus Status { get; set; }
    }
}