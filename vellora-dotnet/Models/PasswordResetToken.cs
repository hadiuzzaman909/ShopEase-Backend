using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vellora.ECommerce.API.Models
{
  public class PasswordResetToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}