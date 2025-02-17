using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;

namespace ShopEase.Services.IServices
{
    public interface IAuthService
    {
            Task<(bool Succeeded, RegisterResponse Result)> RegisterAsync(RegisterRequest request);
            Task<(bool Succeeded, string Message)> ConfirmEmailAsync(string userId, string token);
            Task<(bool Succeeded, LoginResponse Result)> LoginAsync(LoginRequest request);
            Task<(bool Succeeded, object Result)> RefreshTokenAsync(string refreshTokenValue);
            Task<(bool Succeeded, string Message)> LogoutAsync(string refreshTokenValue);
     }
    
}
