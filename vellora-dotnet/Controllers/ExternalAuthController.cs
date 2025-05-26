using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Vellora.ECommerce.API.Services.IServices;

namespace Vellora.ECommerce.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public ExternalAuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet("login/google")]
        public IActionResult ExternalLogin(string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "ExternalAuth", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet("login/google/callback")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
                return BadRequest($"Error from external provider: {remoteError}");

            var result = await HttpContext.AuthenticateAsync("External");
            if (!result.Succeeded)
                return BadRequest("External authentication error.");

            var externalUser = result.Principal;

            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var name = externalUser.FindFirstValue(ClaimTypes.Name);

            var tokenResponse = await _authService.ExternalLoginAsync(email, name);

            await HttpContext.SignOutAsync("External");

            if (!tokenResponse.Succeeded)
                return BadRequest(tokenResponse.Result);

            return Ok(tokenResponse.Result);
        }
    }

}