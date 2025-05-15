using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Repositories
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
