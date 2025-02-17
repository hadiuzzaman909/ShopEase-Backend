using ShopEase.Data;
using ShopEase.Models;
using ShopEase.Repositories.IRepositories;

namespace ShopEase.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
