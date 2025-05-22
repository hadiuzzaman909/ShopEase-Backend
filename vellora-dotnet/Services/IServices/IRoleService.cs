using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;

namespace Vellora.ECommerce.API.Services.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleResponse>> GetAllRolesAsync();
        Task<RoleResponse> GetRoleByIdAsync(string roleId);
        Task<RoleResponse> CreateRoleAsync(RoleRequest request);
        Task<bool> UpdateRoleAsync(string roleId, RoleRequest request);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<bool> AssignPermissionsToRoleAsync(string roleId, List<int> permissionIds);
        Task<bool> RemovePermissionFromRoleAsync(string roleId, int permissionId);
    }
}