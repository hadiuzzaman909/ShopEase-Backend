using ShopEase.Models;

namespace ShopEase.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task AddRangeAsync(IEnumerable<Product> products);
    }
}
