using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace TechShop.Domain.DTOs.JWTDto
{
    public class JwtDto
    {
        public JwtSecurityToken? Token { get; set; }
        public DateTime Expiration { get; set; }
        public string TokenValue { get; set; } = string.Empty;
        public CookieOptions CookieOptions { get; set; } = null!;
    }
}
