using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vellora.ECommerce.API.Data;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;

namespace Vellora.ECommerce.API.Repositories
{
public class PasswordResetTokenRepository : Repository<PasswordResetToken>, IPasswordResetTokenRepository
    {
        public PasswordResetTokenRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<PasswordResetToken?> GetByUserIdAsync(string userId)
        {
            return await dbSet.FirstOrDefaultAsync(p => p.UserId == userId && !p.IsUsed && p.ExpiryDate > DateTime.UtcNow);
        }

        public async Task<PasswordResetToken?> GetByUserIdAndOtpAsync(string userId, string otp)
        {
            return await dbSet.FirstOrDefaultAsync(p => p.UserId == userId && p.Otp == otp && !p.IsUsed && p.ExpiryDate > DateTime.UtcNow);
        }
    }
}