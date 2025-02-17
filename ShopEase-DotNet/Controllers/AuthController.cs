using Microsoft.AspNetCore.Mvc;
using ShopEase.DTOs.Request;
using ShopEase.Services.IServices;
namespace ShopEase.Controllers
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
            var (Succeeded, Message) = await _authService.ConfirmEmailAsync(userId, token);
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            var (Succeeded, Message) = await _authService.LogoutAsync(request.RefreshToken);
            if (!Succeeded) return BadRequest(Message);
            return Ok(Message);
        }
    }
}