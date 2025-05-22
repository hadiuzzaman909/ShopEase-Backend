using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.Repositories.IRepositories;
using Vellora.ECommerce.API.Services.IServices;
using Vellora.ECommerce.API.Utils;

namespace Vellora.ECommerce.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly JwtUtils _jwtUtils;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration config,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            JwtUtils jwtUtils,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _jwtUtils = jwtUtils;
            _httpContextAccessor = httpContextAccessor;

        }

        // 1) REGISTER
        public async Task<(bool Succeeded, RegisterResponse Result)> RegisterAsync(RegisterRequest request)
        {
            var defaultRole = "Customer";

            // Check if default role exists, create if not
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = defaultRole });
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = false,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                Profession = request.Profession,
                IsVerified = false
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                return (false, new RegisterResponse { Message = $"User registration failed: {errors}" });
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, defaultRole);

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            // Determine the scheme (HTTP or HTTPS) dynamically
            // Safely check if HttpContext is available
            var protocol = _httpContextAccessor.HttpContext?.Request.Scheme == "https" ? "https" : "http";

            // Generate the confirmation URL based on the current scheme
            var confirmUrl = $"{protocol}://localhost:5001/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

            var subject = "Confirm Your Account";
            var body = $@"
                <p>Hi {user.FirstName},</p>
                <p>Thank you for registering. Please confirm your account by clicking the link below:</p>
                <p><a href='{confirmUrl}'>Confirm Email</a></p>";

            // Send confirmation email
            await _emailService.SendEmailAsync(user.Email, subject, body);

            return (true, new RegisterResponse
            {
                UserId = user.Id,
                Message = "Registration successful. Please confirm your email via the link sent to your inbox."
            });
        }

        public async Task<(bool Succeeded, string Message)> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Invalid user ID.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Email confirmation failed: {errors}");
            }

            user.IsVerified = true;
            await _userManager.UpdateAsync(user);

            return (true, "Email confirmed successfully. You can now log in.");
        }

        public async Task<(bool Succeeded, LoginResponse Result)> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return (false, new LoginResponse { Message = "Invalid credentials" });

            bool passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return (false, new LoginResponse { Message = "Invalid credentials" });

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtUtils.GenerateJwtToken(user, roles, expiryHours: 1); // 1 hour expiry
            var refreshToken = _jwtUtils.GenerateJwtToken(user, roles, expiryHours: 168); // 7 days expiry

            // Save refresh token in DB
            var refreshTokenEntry = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _unitOfWork.RefreshToken.AddAsync(refreshTokenEntry);
            await _unitOfWork.SaveAsync();

            var response = new LoginResponse
            {
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsVerified = user.IsVerified
            };

            return (true, response);
        }

        public async Task<(bool Succeeded, object Result)> RefreshTokenAsync(string refreshTokenValue)
        {
            var existingToken = await _unitOfWork.RefreshToken.GetAsync(rt => rt.Token == refreshTokenValue && !rt.IsRevoked);
            if (existingToken == null || existingToken.ExpiryDate < DateTime.UtcNow)
                return (false, "Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(existingToken.UserId);
            if (user == null)
                return (false, "User not found.");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtUtils.GenerateJwtToken(user, roles, expiryHours: 1);

            var resultObj = new
            {
                Message = "Token refreshed",
                AccessToken = newAccessToken,
                RefreshToken = existingToken.Token,
                user.IsVerified
            };

            return (true, resultObj);
        }

        public async Task<(bool Succeeded, string Message)> LogoutAsync(string refreshTokenValue)
        {
            var existingToken = await _unitOfWork.RefreshToken.GetAsync(r => r.Token == refreshTokenValue && !r.IsRevoked);
            if (existingToken == null)
                return (false, "Refresh token not found or already revoked.");

            existingToken.IsRevoked = true;
            _unitOfWork.RefreshToken.Update(existingToken);
            await _unitOfWork.SaveAsync();

            return (true, "Logout successful. Refresh token revoked.");
        }

        public async Task<(bool Succeeded, string Message)> SendPasswordResetOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "Email not registered.");

            // Generate 6-digit OTP
            var otp = GenerateOtp();

            // Create OTP record (assuming PasswordResetToken entity and repository exist)
            var otpRecord = new PasswordResetToken
            {
                UserId = user.Id,
                Otp = otp,
                ExpiryDate = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false
            };

            await _unitOfWork.PasswordResetTokens.AddAsync(otpRecord);
            await _unitOfWork.SaveAsync();

            // Send OTP via email
            var subject = "Your Password Reset OTP";
            var body = $"Your OTP code is: {otp}. It expires in 15 minutes.";

            await _emailService.SendEmailAsync(email, subject, body);

            return (true, "OTP sent to your email.");
        }

        public async Task<(bool Succeeded, string Message)> VerifyPasswordResetOtpAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "Email not registered.");

            var otpRecord = await _unitOfWork.PasswordResetTokens.GetByUserIdAndOtpAsync(user.Id, otp);
            if (otpRecord == null || otpRecord.IsUsed || otpRecord.ExpiryDate < DateTime.UtcNow)
                return (false, "Invalid or expired OTP.");

            return (true, "OTP verification successful.");
        }

        public async Task<(bool Succeeded, string Message)> ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
                return (false, "Password and confirmation do not match.");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return (false, "Email not registered.");

            var otpRecord = await _unitOfWork.PasswordResetTokens.GetByUserIdAndOtpAsync(user.Id, request.Otp);
            if (otpRecord == null || otpRecord.IsUsed || otpRecord.ExpiryDate < DateTime.UtcNow)
                return (false, "Invalid or expired OTP.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

            if (!resetResult.Succeeded)
            {
                var errors = string.Join("; ", resetResult.Errors.Select(e => e.Description));
                return (false, $"Password reset failed: {errors}");
            }

            // Mark OTP as used
            otpRecord.IsUsed = true;
            _unitOfWork.PasswordResetTokens.Update(otpRecord);
            await _unitOfWork.SaveAsync();

            return (true, "Password has been reset successfully.");
        }

        // Helper method to generate OTP
        private string GenerateOtp()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var randomNumber = BitConverter.ToUInt32(bytes, 0) % 1000000;
            return randomNumber.ToString("D6");
        }

    }
}