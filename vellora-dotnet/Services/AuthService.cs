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
            // Step 1: Check if the role is valid before creating the user
            var role = string.IsNullOrWhiteSpace(request.Role) ? "Customer" : request.Role;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                return (false, new RegisterResponse { Message = $"Role '{role}' does not exist." });
            }

            // Step 2: Create the user only after validating the role
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
                return (false, new RegisterResponse { Message = "User registration failed." });
            }

            // Step 3: Add the user to the validated role
            await _userManager.AddToRoleAsync(user, role);

            // Step 4: Generate email confirmation token and send email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            // Determine the scheme (HTTP or HTTPS) dynamically
            // Safely check if HttpContext is available
            var protocol = _httpContextAccessor.HttpContext?.Request.Scheme == "https" ? "https" : "http";

            // Generate the confirmation URL based on the current scheme
            var confirmUrl = $"{protocol}://localhost:5001/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

            var subject = "Confirm Your Vellora.ECommerce.API Account";
            var body = $@"
            <p>Hi {user.FirstName},</p>
            <p>Thank you for registering. Please confirm your account by clicking the link below:</p>
            <p><a href='{confirmUrl}'>Confirm Email</a></p>
            ";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            return (true, new RegisterResponse
            {
                UserId = user.Id,
                Message = "Registration successful. Please confirm your email via the link sent to your inbox."
            });
        }

        // 2) CONFIRM EMAIL
        public async Task<(bool Succeeded, string Message)> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Invalid user ID.");

            Console.WriteLine($"ConfirmEmailAsync called with token length: {token?.Length}");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return (false, $"Email confirmation failed: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            user.IsVerified = true;
            await _userManager.UpdateAsync(user);

            return (true, "Email confirmed successfully. You can now log in.");
        }

        // 2) LOGIN
        public async Task<(bool Succeeded, LoginResponse Result)> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return (false, new LoginResponse { Message = "Invalid credentials" });

            bool passValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passValid)
                return (false, new LoginResponse { Message = "Invalid credentials" });


            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtUtils.GenerateJwtToken(user, roles, 1);
            var refreshToken = _jwtUtils.GenerateJwtToken(user, roles, 168);

            var refreshTokenEntry = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _unitOfWork.RefreshToken.AddAsync(refreshTokenEntry);
            await _unitOfWork.SaveAsync();


            var loginResponse = new LoginResponse
            {
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IsVerified = user.IsVerified
            };

            return (true, loginResponse);
        }

        // 4) REFRESH TOKEN
        public async Task<(bool Succeeded, object Result)> RefreshTokenAsync(string refreshTokenValue)
        {
            var existing = await _unitOfWork.RefreshToken.GetByTokenAsync(refreshTokenValue);
            if (existing == null || existing.IsRevoked || existing.ExpiryDate < DateTime.UtcNow)
                return (false, "Invalid or expired refresh token.");

            var user = await _userManager.FindByIdAsync(existing.UserId);
            if (user == null)
                return (false, "User not found.");

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _jwtUtils.GenerateJwtToken(user, roles, 1);

            var resultObj = new
            {
                Message = "Token refreshed",
                AccessToken = newAccessToken,
                RefreshToken = existing.Token,
                user.IsVerified
            };

            return (true, resultObj);
        }


        public async Task<(bool Succeeded, string Message)> SendPasswordResetOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "Email not registered.");

            // Generate 6-digit OTP
            var otp = GenerateOtp();

            // Create OTP record
            var otpRecord = new PasswordResetToken
            {
                UserId = user.Id,
                Otp = otp,
                ExpiryDate = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false
            };

            await _unitOfWork.PasswordResetTokens.AddAsync(otpRecord);
            await _unitOfWork.SaveAsync();

            // Send OTP email
            var subject = "Your Vellora Password Reset OTP";
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
            if (otpRecord == null)
                return (false, "Invalid or expired OTP.");

            // Optionally mark OTP as used here, or do it in reset password API
            // otpRecord.IsUsed = true;
            // _unitOfWork.PasswordResetTokens.Update(otpRecord);
            // await _unitOfWork.SaveAsync();

            return (true, "OTP verification successful.");
        }


        public async Task<(bool Succeeded, string Message)> ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
                return (false, "Password and confirmation do not match.");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return (false, "Email not registered.");

            // Validate OTP
            var otpRecord = await _unitOfWork.PasswordResetTokens.GetByUserIdAndOtpAsync(user.Id, request.Otp);
            if (otpRecord == null)
                return (false, "Invalid or expired OTP.");

            // Reset the password using UserManager
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


        // 5) LOGOUT
        public async Task<(bool Succeeded, string Message)> LogoutAsync(string refreshTokenValue)
        {
            var existing = await _unitOfWork.RefreshToken.GetAsync(r => r.Token == refreshTokenValue && !r.IsRevoked);
            if (existing == null)
                return (false, "Refresh token not found.");

            existing.IsRevoked = true;
            _unitOfWork.RefreshToken.Update(existing);
            await _unitOfWork.SaveAsync();

            return (true, "Logged out (Refresh token revoked).");
        }

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