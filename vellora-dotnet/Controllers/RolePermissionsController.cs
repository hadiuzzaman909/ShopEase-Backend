using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.Services.IServices;

namespace Vellora.ECommerce.API.Controllers
{
    [Route("api/roles/{roleId}/permissions")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolePermissionsController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> AssignPermissions(string roleId, [FromBody] AssignPermissionsRequest request)
        {
            var success = await _roleService.AssignPermissionsToRoleAsync(roleId, request.PermissionIds);
            if (!success) return NotFound("Role not found.");

            return Ok("Permissions assigned successfully.");
        }

        [HttpDelete("{permissionId}")]
        public async Task<IActionResult> RemovePermission(string roleId, int permissionId)
        {
            var success = await _roleService.RemovePermissionFromRoleAsync(roleId, permissionId);
            if (!success) return NotFound("Permission or Role not found.");

            return Ok("Permission removed successfully.");
        }
    }
}