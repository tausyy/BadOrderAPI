using BadOrder.Library.Abstractions.Services;
using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models.Users;
using BadOrder.Library.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace BadOrder.Library.Services
{
    public class AuthService : IAuthService
    {

        private readonly JwtTokenSettings _jwtTokenSettings;
        private readonly byte[] _secret;

        private readonly string _adminRole = "Admin";
        private readonly string _userRole = "User";

        public AuthService(JwtTokenSettings jwtTokenSettings)
        {
            _jwtTokenSettings = jwtTokenSettings;
            _secret = Encoding.UTF8.GetBytes(_jwtTokenSettings.Secret);
        }

        public string AdminRole => _adminRole;
        public string UserRole => _userRole;

        public bool IsAuthRole(string role) =>
            role.ToLower() == _adminRole.ToLower() || role.ToLower() == _userRole.ToLower();

        public string HashPassword(string password) => 
            BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyUserPassword(string loginPassword, string? storedPassword) => 
            string.IsNullOrWhiteSpace(storedPassword) switch
            {
                true => false,
                false => BCrypt.Net.BCrypt.Verify(loginPassword, storedPassword)
            };
            
        public string GenerateJwtToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var jwtToken = new JwtSecurityToken(
                issuer: _jwtTokenSettings.Issuer,
                audience: _jwtTokenSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials:
                    new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
