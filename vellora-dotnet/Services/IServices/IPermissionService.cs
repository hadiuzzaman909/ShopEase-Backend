using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;

namespace Vellora.ECommerce.API.Services.IServices
{
 public interface IPermissionService
    {
        Task<IEnumerable<PermissionResponse>> GetAllPermissionsAsync();
        Task<PermissionResponse> GetPermissionByIdAsync(int id);
        Task<PermissionResponse> CreatePermissionAsync(PermissionRequest request);
        Task<bool> UpdatePermissionAsync(int id, PermissionRequest request);
        Task<bool> DeletePermissionAsync(int id);
    }
}