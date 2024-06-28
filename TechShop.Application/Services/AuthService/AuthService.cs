using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using TechShop.Domain.DTOs.Auth;
using TechShop.Domain.DTOs.JWT;
using TechShop.Domain.DTOs.User;
using TechShop.Domain.Enums;

namespace TechShop.Application.Services.AuthService
{
    public class AuthService(UserManager<ApplicationUser> userManager, 
        IOptions<JwtTokenSettings> jwtTokenSettings, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        public async Task<JwtDto> GenerateJwtAsync(ApplicationUser user)
        {
            var expires = DateTime.UtcNow.AddDays(jwtTokenSettings.Value.JwtExpires);

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = userManager.GetRolesAsync(user).Result;

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Value.JwtKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtTokenSettings.Value.JwtIssuer,
                audience: jwtTokenSettings.Value.JwtAudience,
                signingCredentials: creds,
                expires: expires,
                claims: authClaims
            );

            var jwt = new JwtDto
            {
                Token = token,
                Expiration = expires
            };

            return await Task.FromResult(jwt);
        }
    }
}
