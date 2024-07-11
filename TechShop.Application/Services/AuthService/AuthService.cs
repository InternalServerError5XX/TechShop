using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechShop.Application.Services.UserServices.UserProfileService;
using TechShop.Domain.DTOs.AuthDto;
using TechShop.Domain.DTOs.JWTDto;
using TechShop.Domain.Entities.UserEntities;
using TechShop.Domain.Identity;

namespace TechShop.Application.Services.AuthService
{
    public class AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtTokenSettings> jwtTokenSettings,
        IMapper mapper, IUserProfileService userProfileService) : IAuthService
    {    
        public JwtDto GenerateJwt(ApplicationUser user)
        {
            var expires = DateTime.UtcNow.AddDays(jwtTokenSettings.Value.JwtExpires);

            var authClaims = new List<Claim>
            {
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

            return jwt;
        }

        public async Task<JwtDto> Login(LoginDto loginDto)
        {
            var appUser = await userManager.Users
                    .Include(x => x.UserProfile)
                    .SingleOrDefaultAsync(x => x.Email == loginDto.Email);

            if (appUser == null)
                throw new Exception("User does not exist");

            if (!await userManager.CheckPasswordAsync(appUser, loginDto.Password))
                throw new Exception("Invalid password");

            var token = GenerateJwt(appUser);
            token.TokenValue = new JwtSecurityTokenHandler().WriteToken(token.Token);

            appUser.UserProfile.LastLogin = DateTime.Now;
            await userProfileService.UpdateAsync(appUser.UserProfile);

            token.CookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true,
                Expires = token.Expiration
            };

            return token;
        }

        public async Task<JwtDto> Register(RegisterDto registerDto)
        {
            var appUser = await userManager.FindByEmailAsync(registerDto.Email);

            if (appUser != null)
                throw new Exception("User is already exists");

            var newUser = mapper.Map<ApplicationUser>(registerDto);

            var response = await userManager.CreateAsync(newUser, registerDto.Password);

            if (!response.Succeeded)
            {
                var errorsMessages =
                    response.Errors.Aggregate("", (current, error) => current + " " + error.Description);

                throw new Exception(errorsMessages);
            }

            await userManager.AddToRoleAsync(newUser, DefaultRoles.User);

            var profile = mapper.Map<UserProfile>(registerDto);
            profile.UserId = newUser.Id;
            await userProfileService.UpdateAsync(profile);

            var user = mapper.Map<LoginDto>(registerDto);
            return await Login(user);
        }
    }
}
