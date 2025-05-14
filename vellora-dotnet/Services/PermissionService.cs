using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopEase.Models;
using ShopEase.Repositories.IRepositories;
using ShopEase.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopEase.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // ✅ Check if user has a specific permission
        public async Task<bool> HasPermissionAsync(string userId, string permissionName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            var roleIds = await _unitOfWork.Permissions
                .GetQueryable()
                .Where(rp => roles.Contains(rp.RolePermissions.Select(r => r.Role.Name).FirstOrDefault()))
                .Select(rp => rp.Id)
                .Distinct()
                .ToListAsync();

            return await _unitOfWork.Permissions
                .GetQueryable()
                .AnyAsync(p => roleIds.Contains(p.Id) && p.Name == permissionName);
        }

        // ✅ Retrieve all permissions
        public async Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return await _unitOfWork.Permissions.GetAllAsync();
        }

        // ✅ Add a new permission
        public async Task<Permission> AddPermissionAsync(Permission permission)
        {
            if (await _unitOfWork.Permissions.ExistsAsync(p => p.Name == permission.Name))
            {
                throw new InvalidOperationException("Permission already exists.");
            }

            await _unitOfWork.Permissions.AddAsync(permission);
            await _unitOfWork.SaveAsync();
            return permission;
        }

        // ✅ Update an existing permission
        public async Task<Permission> UpdatePermissionAsync(int id, Permission updatedPermission)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission == null)
            {
                throw new KeyNotFoundException("Permission not found.");
            }

            permission.Name = updatedPermission.Name;
            permission.Description = updatedPermission.Description;

            _unitOfWork.Permissions.Update(permission);
            await _unitOfWork.SaveAsync();
            return permission;
        }

        // ✅ Delete a permission
        public async Task<bool> DeletePermissionAsync(int id)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(id);
            if (permission == null)
            {
                return false;
            }

            _unitOfWork.Permissions.Remove(permission);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}

