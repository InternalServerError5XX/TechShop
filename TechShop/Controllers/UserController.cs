using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.UserDtos.UserProfileDto;
using TechShop.Domain.Entities;

namespace TechShopWeb.Controllers
{
    [Authorize]
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class UserController(IUserService userService, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profile = await userService.GetUserProfile(email!);
            var response = mapper.Map<ResponseUserProfileDto>(profile);

            return PartialView("~/Views/Shared/_ProfileModal.cshtml", response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ResponseUserProfileDto userProfileDto)
        {
            var profile = mapper.Map<UserProfile>(userProfileDto);
            await userService.UpdateProfile(profile);

            return Ok();
        }
    }
}
