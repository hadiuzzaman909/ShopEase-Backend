using ShopEase.Models;

namespace ShopEase.Repositories.IRepositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
    }
}
