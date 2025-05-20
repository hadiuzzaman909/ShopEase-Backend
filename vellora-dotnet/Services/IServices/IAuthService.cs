using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;

namespace Vellora.ECommerce.API.Services.IServices
{
    public interface IAuthService
    {
            Task<(bool Succeeded, RegisterResponse Result)> RegisterAsync(RegisterRequest request);
            Task<(bool Succeeded, string Message)> ConfirmEmailAsync(string userId, string token);
            Task<(bool Succeeded, LoginResponse Result)> LoginAsync(LoginRequest request);
            Task<(bool Succeeded, object Result)> RefreshTokenAsync(string refreshTokenValue);
            Task<(bool Succeeded, string Message)> SendPasswordResetOtpAsync(string email);

            Task<(bool Succeeded, string Message)> VerifyPasswordResetOtpAsync(string email, string otp);

            Task<(bool Succeeded, string Message)> ResetPasswordAsync(ResetPasswordRequest request);

            Task<(bool Succeeded, string Message)> LogoutAsync(string refreshTokenValue);
     }
    
}
