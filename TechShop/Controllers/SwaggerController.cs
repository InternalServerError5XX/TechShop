using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AuthService;
using TechShop.Domain.DTOs.Auth;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using TechShop.Domain.DTOs.User;
using TechShop.Domain.Identity;
using TechShop.Domain.Entities;

namespace TechShop.Controllers
{
    public class SwaggerController(UserManager<ApplicationUser> userManager, IAuthService authService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [HttpGet("AuthCheck")]
        public IActionResult AuthCheck()
        {
            try
            {
                var token = Request.Cookies["token"];

                if (token == null)
                    return Unauthorized("Unauthorized");

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Wrong input");

                var appUser = await userManager.FindByEmailAsync(loginDto.Email);

                if (appUser == null)
                    return NotFound("User does not exist");

                if (!await userManager.CheckPasswordAsync(appUser, loginDto.Password))
                    return BadRequest("Invalid password");

                var token = await authService.GenerateJwtAsync(appUser);
                var tokenValue = new JwtSecurityTokenHandler().WriteToken(token.Token);

                CookieOptions cookieOptions = new()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    Expires = token.Expiration
                };

                Response.Cookies.Append(
                    "token",
                    tokenValue,
                    cookieOptions);

                return Ok(new
                {
                    Token = tokenValue,
                    Expires = token.Expiration
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Wrong input");

                var appUser = await userManager.FindByEmailAsync(registerDto.Email);

                if (appUser != null)
                    return NotFound("User is already exists");

                var user = new ApplicationUser
                {
                    Email = registerDto.Email,
                    UserName = registerDto.Email.Substring(0, registerDto.Email.IndexOf('@')),                    
                };

                var response = await userManager.CreateAsync(user, registerDto.Password);

                if (!response.Succeeded)
                {
                    var errorsMessages =
                        response.Errors.Aggregate("", (current, error) => current + " " + error.Description);

                    return BadRequest(errorsMessages);
                }

                await userManager.AddToRoleAsync(user, DefaultRoles.User);

                var profile = new UserProfile
                {
                    Firstname = registerDto.Firstname,
                    Lastname = registerDto.Lastname,
                    Age = registerDto.Age,
                };

                var newUser = new LoginDto
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password
                };

                return Ok(await Login(newUser));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                Response.Cookies.Delete("token");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
