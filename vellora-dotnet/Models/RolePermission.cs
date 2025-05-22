namespace Vellora.ECommerce.API.Models
{
 public class RolePermission
    {
        // Foreign key to ApplicationRole
        public string RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }

        // Foreign key to Permission
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
