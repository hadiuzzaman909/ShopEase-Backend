using Vellora.ECommerce.API.Models;

namespace Vellora.ECommerce.API.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task AddRangeAsync(IEnumerable<Product> products);
    }
}
