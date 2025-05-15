using Microsoft.IdentityModel.Tokens;
using Vellora.ECommerce.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Vellora.ECommerce.API.Utils
{
    public class JwtUtils
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtUtils(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["JwtSettings:SecretKey"] ?? throw new ArgumentNullException(nameof(config), "JwtSettings:SecretKey is missing in configuration.")));

            _issuer = _config["JwtSettings:Issuer"] ?? throw new ArgumentNullException(nameof(config), "JwtSettings:Issuer is missing in configuration.");
            _audience = _config["JwtSettings:Audience"] ?? throw new ArgumentNullException(nameof(config), "JwtSettings:Audience is missing in configuration.");
        }

        public string GenerateJwtToken(ApplicationUser user, IList<string> roles, int expiryHours)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiryHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}