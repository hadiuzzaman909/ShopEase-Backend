using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.Models;
using ShopEase.Repositories.IRepositories;

namespace ShopEase.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _db;

        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _db.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product) // Ensure Product details are included
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
