using Vellora.ECommerce.API.Models;

namespace Vellora.ECommerce.API.Repositories.IRepositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
    }
}