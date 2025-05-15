using Microsoft.AspNetCore.Identity;

namespace Vellora.ECommerce.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Profession { get; set; }
        public bool IsVerified { get; set; } = false;

        // Establish 1-to-1 relationship with Cart (Each user has ONE cart)
        public virtual Cart Cart { get; set; }

        // Establish 1-to-Many relationship with Orders (Each user can place multiple orders)
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Define Many-to-Many relationship for Roles
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();
    }
}
