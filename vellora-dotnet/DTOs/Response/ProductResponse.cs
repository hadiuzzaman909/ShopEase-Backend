namespace Vellora.ECommerce.API.DTOs.Response
{
 public class ProductResponse
    {
        public int Id { get; set; }  
        public string Name { get; set; } 
        public string Description { get; set; }  
        public decimal Price { get; set; }  
        public int StockQuantity { get; set; } 
        public bool IsAvailable { get; set; }  
        public string SKU { get; set; } 
        public string Brand { get; set; }  
        public string ImageUrl { get; set; } 
        public decimal DiscountPercentage { get; set; } 
        public decimal DiscountedPrice { get; set; } 
        public DateTime CreatedAt { get; set; }  
        public DateTime? UpdatedAt { get; set; } 
        public CategoryResponse Category { get; set; }  
    }
}