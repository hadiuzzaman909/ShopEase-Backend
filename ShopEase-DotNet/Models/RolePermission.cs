using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopEase.Models
{
    public class RolePermission
    {
        // ✅ Composite Key
        [Required]
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public ApplicationRole Role { get; set; }

        [Required]
        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }
    }
}
