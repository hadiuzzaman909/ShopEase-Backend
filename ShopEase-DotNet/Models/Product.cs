using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopEase.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }  
        public bool IsAvailable { get; set; }   

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }


        public string? Brand { get; set; } 


        public string ImageUrl { get; set; }

        public double? Rating { get; set; }  

        public decimal DiscountPercentage { get; set; }
        public decimal DiscountedPrice { get; set; }   

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public bool IsVisible { get; set; } = true;

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}