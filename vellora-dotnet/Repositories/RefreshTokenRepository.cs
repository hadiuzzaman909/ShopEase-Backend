using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Repositories
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