using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.UserDtos.RoleDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;

namespace TechShopWeb.Controllers
{
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class RoleController(IUserService userService) : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("~/Views/Admin/User/_CreateRoleModal.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestRoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return PartialView("~/Views/Admin/User/_CreateRoleModal.cshtml", roleDto);

            await userService.CreateRole(roleDto);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var response = await userService.GetRole(id);
            return PartialView("~/Views/Admin/User/_CreateRoleModal.cshtml", response);
        }

        [HttpPatch]
        public async Task<IActionResult> Update(IdentityRole identityRole)
        {
            await userService.UpdateRole(identityRole);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var response = await userService.GetRole(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await userService.GetRoles();
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await userService.DeleteRole(id);
            return NoContent();
        }
    }
}
