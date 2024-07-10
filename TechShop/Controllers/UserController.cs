using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.UserDtos.UserDto;
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

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("~/Views/Admin/User/_CreateUserModal.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestUserDto userDto)
        {
            if (!ModelState.IsValid)
                return PartialView("~/Views/Admin/User/_CreateUserModal.cshtml", userDto);

            var user = await userService.CreateUser(userDto);
            var response = mapper.Map<ApplicationUserDto>(user);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Update(ResponseUserProfileDto userProfileDto)
        {
            var profile = mapper.Map<UserProfile>(userProfileDto);
            await userService.UpdateProfile(profile);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string userId)
        {
            var profile = await userService.GetProfile(userId);
            var response = mapper.Map<ResponseUserProfileDto>(profile);
            response.Role = await userService.GetRoleByUserId(userId);
            response.RoleId = response.Role.Id;

            return PartialView("~/Views/Admin/User/_UpdateProfileModal.cshtml", response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser(ResponseUserProfileDto profileDto)
        {
            var profile = mapper.Map<UpdateUserDto>(profileDto);            
            await userService.UpdateUser(profile);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await userService.DeleteUser(id);
            return NoContent();
        }
    }
}
