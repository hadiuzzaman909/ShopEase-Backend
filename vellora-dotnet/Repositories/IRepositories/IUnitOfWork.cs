﻿namespace Vellora.ECommerce.API.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IRefreshTokenRepository RefreshToken { get; }
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IPermissionRepository Permissions { get; }
        ICartRepository Cart { get; }
        IOrderRepository Order { get; }
        IPasswordResetTokenRepository PasswordResetTokens { get; }
        Task SaveAsync();
    }
}