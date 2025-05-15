namespace Vellora.ECommerce.API.DTOs.Request
{
    public class OrderRequest
    {
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
    }
}
