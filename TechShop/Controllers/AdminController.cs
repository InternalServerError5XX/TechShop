using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.AppServices.CacheService;
using TechShop.Domain.DTOs.AdminDto;

namespace TechShopWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class AdminController(IAdminService adminService) : Controller
    {
        [HttpGet]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAdminPanel()
        {
            var response = await adminService.GetCachedAdminPanel();
            return PartialView("~/Views/Admin/User/_UsersPartial.cshtml", response.Users);
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesAdminPanel()
        {
            var response = await adminService.GetCachedAdminPanel();
            return PartialView("~/Views/Admin/User/_RolesPartial.cshtml", response.Roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAdminPanel()
        {
            var response = await adminService.GetCachedAdminPanel();
            return PartialView("~/Views/Admin/Product/_ProductsPartial.cshtml", response.Products);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesAdminPanel()
        {
            var response = await adminService.GetCachedAdminPanel();
            return PartialView("~/Views/Admin/Product/_CategoriesPartial.cshtml", response.Categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersAdminPanel()
        {
            var response = await adminService.GetCachedAdminPanel();
            return PartialView("~/Views/Admin/Order/_OrdersPartial.cshtml", response.Orders);
        }

        [HttpGet]
        public IActionResult Logs()
        {
            return View();
        }
    }
}
