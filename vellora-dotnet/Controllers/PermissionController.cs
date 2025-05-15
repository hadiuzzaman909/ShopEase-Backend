using Microsoft.AspNetCore.Mvc;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Vellora.ECommerce.API.Services.IServices;
using Vellora.ECommerce.API.Models;


namespace Vellora.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        // ✅ Get all permissions
        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _permissionService.GetPermissionsAsync();
            var response = permissions.Select(p => new PermissionResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            });
            return Ok(response);
        }

        // ✅ Add a new permission
        [HttpPost]
        public async Task<IActionResult> AddPermission([FromBody] PermissionRequest permissionRequest)
        {
            try
            {
                // Mapping PermissionRequest to Permission
                var permission = new Permission
                {
                    Name = permissionRequest.Name,
                    Description = permissionRequest.Description
                };

                var newPermission = await _permissionService.AddPermissionAsync(permission);

                var response = new PermissionResponse
                {
                    Id = newPermission.Id,
                    Name = newPermission.Name,
                    Description = newPermission.Description
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Update an existing permission
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionRequest permissionRequest)
        {
            try
            {
                var updatedPermission = new Permission
                {
                    Name = permissionRequest.Name,
                    Description = permissionRequest.Description
                };

                var permission = await _permissionService.UpdatePermissionAsync(id, updatedPermission);

                var response = new PermissionResponse
                {
                    Id = permission.Id,
                    Name = permission.Name,
                    Description = permission.Description
                };

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // ✅ Delete a permission
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            bool deleted = await _permissionService.DeletePermissionAsync(id);
            if (!deleted) return NotFound("Permission not found.");

            return Ok("Permission deleted.");
        }
    }
}
