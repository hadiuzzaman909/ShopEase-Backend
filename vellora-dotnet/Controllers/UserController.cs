using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.Services.IServices;

namespace Vellora.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Helper to get current user's ID safely
        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User ID claim not found");
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetUserId();
            var profile = await _userService.GetUserProfileAsync(userId);
            if (profile == null)
                return NotFound(new { message = "User not found." });

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            var (Succeeded, Message) = await _userService.UpdateUserProfileAsync(userId, request);

            if (!Succeeded)
                return BadRequest(new { message = Message });

            return Ok(new { message = Message });
        }

        // New endpoint to get all customers, secured by permission
        [HttpGet("Users")]
        [Authorize(Policy = "Permission.ViewUsers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _userService.GetAllCustomersAsync();
            return Ok(customers);
        }
    }
}