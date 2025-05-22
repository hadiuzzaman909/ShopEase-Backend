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
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var (succeeded, result) = await _authService.RegisterAsync(request);
            if (!succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return BadRequest("UserId and token are required");

            string decodedToken = Uri.UnescapeDataString(token);

            var (Succeeded, Message) = await _authService.ConfirmEmailAsync(userId, decodedToken);
            if (!Succeeded) return BadRequest(Message);

            return Ok(Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (Succeeded, Result) = await _authService.LoginAsync(request);
            if (!Succeeded) return Unauthorized(Result);
            return Ok(Result);
        }

        // POST: api/auth/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var (Succeeded, Result) = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (!Succeeded) return Unauthorized(Result);
            return Ok(Result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var (Succeeded, Message) = await _authService.LogoutAsync(request.RefreshToken);
            if (!Succeeded) return BadRequest(Message);
            return Ok(Message);
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


    }
}