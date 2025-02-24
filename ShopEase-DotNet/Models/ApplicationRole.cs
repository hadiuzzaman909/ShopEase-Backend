using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ShopEase.Models
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}

