using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Services.IServices;

namespace Vellora.ECommerce.API.Services
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserPermissionService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permissionName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(permissionName))
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0)
                return false;

            var permission = await _dbContext.Permissions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == permissionName);

            if (permission == null)
                return false;

            // Join RolePermissions with roles to check if any role contains the permission
            var hasPermission = await _dbContext.RolePermissions
                .AsNoTracking()
                .Include(rp => rp.Role)
                .Where(rp => roles.Contains(rp.Role.Name) && rp.PermissionId == permission.Id)
                .AnyAsync();

            return hasPermission;
        }
    }
}