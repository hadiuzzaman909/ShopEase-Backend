using Microsoft.AspNetCore.Identity;

namespace ShopEase.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}