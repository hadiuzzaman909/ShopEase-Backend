using ShopEase.Models;

namespace ShopEase.Services.IServices
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string userId, string permissionName);

        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<Permission> AddPermissionAsync(Permission permission);
        Task<Permission> UpdatePermissionAsync(int id, Permission updatedPermission);
        Task<bool> DeletePermissionAsync(int id);
    }
}