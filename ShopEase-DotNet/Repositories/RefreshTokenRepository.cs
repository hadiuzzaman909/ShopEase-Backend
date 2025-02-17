using Microsoft.EntityFrameworkCore;
using ShopEase.Data;
using ShopEase.Models;
using ShopEase.Repositories.IRepositories;

namespace ShopEase.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _db;

        public RefreshTokenRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var refreshToken = await _db.Set<RefreshToken>().FirstOrDefaultAsync(r => r.Token == token);
            return refreshToken ?? throw new InvalidOperationException("Refresh token not found.");
        }

    }
}