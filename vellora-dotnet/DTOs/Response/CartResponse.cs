namespace ShopEase.DTOs.Response
{
    public class CartResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemResponse> CartItems { get; set; } = new List<CartItemResponse>();
        public decimal TotalAmount => CartItems.Sum(item => item.TotalPrice);
    }
}
