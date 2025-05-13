using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopEase.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount => OrderItems.Sum(oi => oi.TotalPrice); 

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public string ShippingAddress { get; set; }
        [Required]
        public string PaymentMethod { get; set; } 

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

}
