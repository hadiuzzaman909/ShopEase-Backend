using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopEase.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }  // ✅ Establish relationship with User

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
