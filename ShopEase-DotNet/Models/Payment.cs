namespace ShopEase.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string PaymentStatus { get; set; } // "Pending", "Completed", "Failed"
        public decimal TotalAmount { get; set; }

        public Order Order { get; set; }
    }
}
