using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vellora.ECommerce.API.Models;

namespace Vellora.ECommerce.API.Repositories.IRepositories
{
  public interface IPasswordResetTokenRepository : IRepository<PasswordResetToken>
    {
        Task<PasswordResetToken?> GetByUserIdAsync(string userId);
        Task<PasswordResetToken?> GetByUserIdAndOtpAsync(string userId, string otp);
    }
}