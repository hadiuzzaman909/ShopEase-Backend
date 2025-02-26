namespace ShopEase.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IRefreshTokenRepository RefreshToken { get; }
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IPermissionRepository Permissions { get; }
        ICartRepository Cart { get; }
        IOrderRepository Order { get; }
        Task SaveAsync();
    }
}