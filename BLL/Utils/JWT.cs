using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Utils
{
    public static class JwtConfig 
    {
        public static string JwtKey => "SOME_RANDOM_KEY_DO_NOT_SHARE";
        public static int JwtExpireDays => 30;
        public static string JwtIssuer => "https://localhost:44325";
    }
    public static class JWT
    {
        public static string GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(JwtConfig.JwtExpireDays);

            var token = new JwtSecurityToken(JwtConfig.JwtIssuer, JwtConfig.JwtIssuer, claims, expires: expires,signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
