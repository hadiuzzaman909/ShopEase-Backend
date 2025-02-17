namespace ShopEase.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // "Pending", "Shipped", "Delivered", "Cancelled"

        //public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
