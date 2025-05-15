using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IRefreshTokenRepository RefreshToken { get; private set; }
        public IPermissionRepository Permissions { get; private set; }

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public ICartRepository Cart { get; private set; }
        public IOrderRepository Order { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            RefreshToken = new RefreshTokenRepository(_db);
            Permissions = new PermissionRepository(_db);
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Cart= new CartRepository(_db);
            Order = new OrderRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}