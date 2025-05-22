using Microsoft.AspNetCore.Identity;

namespace Vellora.ECommerce.API.Models
{
    public class ApplicationRole : IdentityRole
    {
        // Many-to-many relationship with RolePermissions (for role-based permissions)
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        // Many-to-many relationship with Role Claims
        public virtual ICollection<IdentityRoleClaim<string>> RoleClaims { get; set; } = new List<IdentityRoleClaim<string>>();
    }
}