using Microsoft.AspNetCore.Identity;

namespace Vellora.ECommerce.API.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}