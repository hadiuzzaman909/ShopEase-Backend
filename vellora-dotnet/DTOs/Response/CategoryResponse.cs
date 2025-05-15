namespace Vellora.ECommerce.API.DTOs.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Only list the immediate subcategories (no nested subcategories)
        public List<SubCategoryResponse> SubCategories { get; set; } = new List<SubCategoryResponse>();
    }

    public class SubCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}


