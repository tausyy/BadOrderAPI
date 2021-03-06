using BadOrder.Library.Abstractions.Authentication;
using BadOrder.Library.Abstractions.DataAccess;
using BadOrder.Library.Models;
using BadOrder.Library.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Services
{
    public class AuthService
    {

        private readonly JwtTokenSettings _jwtTokenSettings;
        private readonly byte[] _secret;

        public AuthService(JwtTokenSettings jwtTokenSettings)
        {
            _jwtTokenSettings = jwtTokenSettings;
            _secret = Encoding.UTF8.GetBytes(_jwtTokenSettings.Secret);
        }

        public bool VerifyUserPassword(string loginPassword, string storedPassword) =>
            BCrypt.Net.BCrypt.Verify(loginPassword, storedPassword);

        public string GenerateJwtToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
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
