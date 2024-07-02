using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AuthService;
using TechShop.Domain.DTOs.AuthDto;

namespace TechShop.Controllers
{
    public class IdentityController(IAuthService authService) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var token = await authService.Login(loginDto);

            Response.Cookies.Append(
                "token",
                token.TokenValue,
                token.CookieOptions);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var token = await authService.Register(registerDto);

            return RedirectToAction("Index", "Home");
        }
    }
}
