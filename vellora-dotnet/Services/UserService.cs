using Microsoft.AspNetCore.Identity;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Services.IServices;


namespace Vellora.ECommerce.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            UserManager<ApplicationUser> userManager
        )
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserProfileResponse>> GetAllCustomersAsync()
        {
            var customers = await _userManager.GetUsersInRoleAsync("Customer");

            var customerProfiles = customers.Select(user => new UserProfileResponse
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Profession = user.Profession,
                IsVerified = user.IsVerified
            });

            return customerProfiles;
        }

        public async Task<UserProfileResponse> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserProfileResponse
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Profession = user.Profession,
                IsVerified = user.IsVerified
            };
        }

        public async Task<(bool Succeeded, string Message)> UpdateUserProfileAsync(string userId, UpdateUserProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.DateOfBirth = request.DateOfBirth;
            user.Profession = request.Profession;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Failed to update profile: {errors}");
            }

            return (true, "Profile updated successfully.");
        }
    }
}