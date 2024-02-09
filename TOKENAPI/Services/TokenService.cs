using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TOKENAPI.Models;

namespace TOKENAPI.Services
{
    public class TokenService
    {
        public static string GenerateJSONWebToken(IConfiguration configuration, User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email",user.Email),
                new Claim("UserName",user.UserName),
                new Claim("Status",(user.Status == true ? "Online":"Offline")),
                new Claim("LastLoginTime",user.LastLoginTime.ToString()),
                new Claim("AvatarUrl", user.AvatarUrl),
                new Claim("Role",user.Role),
                new Claim(ClaimTypes.Role,user.Role),
            };
            var token = new JwtSecurityToken(configuration["Jwt:Key"],
                configuration["Jwt.Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
