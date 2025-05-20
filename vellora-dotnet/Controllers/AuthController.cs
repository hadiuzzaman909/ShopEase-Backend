using Microsoft.AspNetCore.Mvc;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.Services.IServices;
namespace Vellora.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var (Succeeded, Result) = await _authService.RegisterAsync(request);
            if (!Succeeded) return BadRequest(Result);
            return Ok(Result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("UserId and token are required");

            // Decode the token before passing it
            string decodedToken = Uri.UnescapeDataString(token);

            var (Succeeded, Message) = await _authService.ConfirmEmailAsync(userId, decodedToken);
            if (!Succeeded) return BadRequest(Message);
            return Ok(Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var (Succeeded, Result) = await _authService.LoginAsync(request);
            if (!Succeeded) return Unauthorized(Result);
            return Ok(Result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var (Succeeded, Result) = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (!Succeeded) return Unauthorized(Result);
            return Ok(Result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var (Succeeded, Message) = await _authService.SendPasswordResetOtpAsync(request.Email);
            if (!Succeeded) return BadRequest(Message);

            return Ok(Message);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var (Succeeded, Message) = await _authService.VerifyPasswordResetOtpAsync(request.Email, request.Otp);
            if (!Succeeded) return BadRequest(Message);

            return Ok(Message);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var (Succeeded, Message) = await _authService.ResetPasswordAsync(request);
            if (!Succeeded) return BadRequest(Message);

            return Ok(Message);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            var (Succeeded, Message) = await _authService.LogoutAsync(request.RefreshToken);
            if (!Succeeded) return BadRequest(Message);
            return Ok(Message);
        }
    }
}