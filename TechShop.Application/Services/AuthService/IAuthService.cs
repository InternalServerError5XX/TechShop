using System.Security.Claims;
using TechShop.Domain.DTOs.AuthDto;
using TechShop.Domain.DTOs.JWTDto;
using TechShop.Domain.DTOs.UserDto;

namespace TechShop.Application.Services.AuthService
{
    public interface IAuthService
    {
        JwtDto GenerateJwt(ApplicationUser user);
        Task<JwtDto> Login(LoginDto loginDto);
        Task<JwtDto> Register(RegisterDto registerDto);
    }
}
