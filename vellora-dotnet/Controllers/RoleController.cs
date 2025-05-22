using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Services.IServices;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]  // Only admin can manage roles
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var role = await _roleService.GetRoleByIdAsync(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleRequest request)
    {
        var role = await _roleService.CreateRoleAsync(request);
        return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleRequest request)
    {
        var success = await _roleService.UpdateRoleAsync(id, request);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var success = await _roleService.DeleteRoleAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/permissions")]
    public async Task<IActionResult> AssignPermissions(string id, [FromBody] List<int> permissionIds)
    {
        var success = await _roleService.AssignPermissionsToRoleAsync(id, permissionIds);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermission(string id, int permissionId)
    {
        var success = await _roleService.RemovePermissionFromRoleAsync(id, permissionId);
        if (!success) return NotFound();
        return NoContent();
    }
}
