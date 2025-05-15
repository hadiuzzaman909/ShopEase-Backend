using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Repositories
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