using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Repositories
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
