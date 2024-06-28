using System.Security.Claims;
using TechShop.Domain.DTOs.Auth;
using TechShop.Domain.DTOs.JWT;
using TechShop.Domain.DTOs.User;

namespace TechShop.Application.Services.AuthService
{
    public interface IAuthService
    {
        Task<JwtDto> GenerateJwtAsync(ApplicationUser user);
    }
}
