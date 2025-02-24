using ShopEase.Models;

namespace ShopEase.Repositories.IRepositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
    }
}
