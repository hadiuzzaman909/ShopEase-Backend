using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Services.IServices;

namespace Vellora.ECommerce.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<RoleResponse>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(r => new RoleResponse
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        public async Task<RoleResponse> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return null;

            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<RoleResponse> CreateRoleAsync(RoleRequest request)
        {
            if (await _roleManager.RoleExistsAsync(request.Name))
                throw new Exception("Role already exists.");

            var role = new ApplicationRole { Name = request.Name };
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public async Task<bool> UpdateRoleAsync(string roleId, RoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return false;

            role.Name = request.Name;
            var result = await _roleManager.UpdateAsync(role);

            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return false;

            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded;
        }

        public async Task<bool> AssignPermissionsToRoleAsync(string roleId, List<int> permissionIds)
        {
            var role = await _dbContext.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null) return false;

            foreach (var permissionId in permissionIds)
            {
                if (!role.RolePermissions.Any(rp => rp.PermissionId == permissionId))
                {
                    role.RolePermissions.Add(new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    });
                }
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(string roleId, int permissionId)
        {
            var rolePermission = await _dbContext.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null) return false;

            _dbContext.RolePermissions.Remove(rolePermission);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}