namespace ShopEase.DTOs.Request
{
    public class CategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SubCategoryRequest> SubCategories { get; set; } 
    }

    public class SubCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}