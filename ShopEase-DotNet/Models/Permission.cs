using System.ComponentModel.DataAnnotations;

namespace ShopEase.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}

