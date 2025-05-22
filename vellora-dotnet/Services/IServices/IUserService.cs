using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;

namespace Vellora.ECommerce.API.Services.IServices
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetUserProfileAsync(string userId);

        Task<IEnumerable<UserProfileResponse>> GetAllCustomersAsync();

        Task<(bool Succeeded, string Message)> UpdateUserProfileAsync(string userId, UpdateUserProfileRequest request);
    }
}