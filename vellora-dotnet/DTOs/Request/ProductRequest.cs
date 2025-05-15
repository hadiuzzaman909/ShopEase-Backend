namespace Vellora.ECommerce.API.DTOs.Request
{
 public class ProductRequest
    {
        public string Name { get; set; }  
        public string Description { get; set; }  
        public decimal Price { get; set; } 
        public int CategoryId { get; set; }  
        public int StockQuantity { get; set; } 
        public bool IsAvailable { get; set; }  
        public string SKU { get; set; }  
        public string Brand { get; set; }  
        public string ImageUrl { get; set; } 
        public decimal DiscountPercentage { get; set; }  
        public decimal DiscountedPrice { get; set; } 
    }
}