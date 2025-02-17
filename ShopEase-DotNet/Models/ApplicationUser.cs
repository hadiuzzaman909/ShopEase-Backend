using Microsoft.AspNetCore.Identity;

namespace ShopEase.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Profession { get; set; }
        public bool IsVerified { get; set; } = false;

        // ✅ Define relationship: A User can have multiple roles
        public ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();
    }
}
