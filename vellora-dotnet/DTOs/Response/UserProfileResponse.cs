using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vellora.ECommerce.API.DTOs.Response
{
    public class UserProfileResponse
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string Profession { get; set; }
    public bool IsVerified { get; set; }
}
}