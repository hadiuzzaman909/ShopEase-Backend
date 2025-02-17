using ShopEase.Data;
using ShopEase.Repositories.IRepositories;

namespace ShopEase.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IRefreshTokenRepository RefreshToken { get; private set; }
        public IPermissionRepository Permissions { get; private set; }

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }



        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            RefreshToken = new RefreshTokenRepository(_db);
            Permissions = new PermissionRepository(_db);
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}