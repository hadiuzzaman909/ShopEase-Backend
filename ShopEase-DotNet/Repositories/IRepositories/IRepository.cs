using System.Linq.Expressions;

namespace ShopEase.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<T?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetQueryable();
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}

