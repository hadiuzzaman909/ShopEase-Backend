using Vellora.ECommerce.API.Models;

namespace Vellora.ECommerce.API.Repositories.IRepositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
    }
}
