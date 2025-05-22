using Microsoft.AspNetCore.Identity;

namespace Vellora.ECommerce.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Profession { get; set; }
        public bool IsVerified { get; set; } = true;

        // 1-to-1 relationship with Cart (Each user has ONE cart)
        public virtual Cart Cart { get; set; }

        // 1-to-many relationship with Orders (Each user can have multiple orders)
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Many-to-many relationship with Roles
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();

        // Many-to-many relationship with Claims
        public virtual ICollection<IdentityUserClaim<string>> UserClaims { get; set; } = new List<IdentityUserClaim<string>>();
        
        // Many-to-many relationship with Logins (for external login)
        public virtual ICollection<IdentityUserLogin<string>> UserLogins { get; set; } = new List<IdentityUserLogin<string>>();
    }
}