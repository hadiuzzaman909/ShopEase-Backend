using ShopEase.Data;
using ShopEase.Models;
using ShopEase.Repositories.IRepositories;

namespace ShopEase.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public PermissionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}

