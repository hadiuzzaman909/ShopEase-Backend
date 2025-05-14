using ShopEase.Data;
using ShopEase.Models;
using ShopEase.Repositories.IRepositories;

namespace ShopEase.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task AddRangeAsync(IEnumerable<Product> products)
        {
            await _db.Products.AddRangeAsync(products);
        }

    }
}