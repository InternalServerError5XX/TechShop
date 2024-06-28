using System.IdentityModel.Tokens.Jwt;

namespace TechShop.Domain.DTOs.JWT
{
    public class JwtDto
    {
        public JwtSecurityToken? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
