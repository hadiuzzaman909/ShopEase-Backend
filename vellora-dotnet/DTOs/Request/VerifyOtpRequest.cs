using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vellora.ECommerce.API.DTOs.Request
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!; 
    }
}