using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.AppServices.CacheService;
using TechShop.Domain.DTOs.AdminDto;

namespace TechShopWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class AdminController(IAdminService adminService, ICacheService cacheService) : Controller
    {
        private readonly string cacheKey = "AdminPanelCacheKey";

        private async Task<ResponseAdminDto> GetCachedAdminPanel()
        {
            var response = cacheService.Get<ResponseAdminDto>(cacheKey);
            if (response == null)
            {
                response = await adminService.GetAdminPanel();
                cacheService.Set(cacheKey, response, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(2));
            }

            return response;
        }

        [HttpGet]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAdminPanel()
        {
            var response = await GetCachedAdminPanel();
            return PartialView("~/Views/Admin/_UsersPartial.cshtml", response.Users);
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesAdminPanel()
        {
            var response = await GetCachedAdminPanel();
            return PartialView("~/Views/Admin/_RolesPartial.cshtml", response.Roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAdminPanel()
        {
            var response = await GetCachedAdminPanel();
            return PartialView("~/Views/Admin/_ProductsPartial.cshtml", response.Products);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesAdminPanel()
        {
            var response = await GetCachedAdminPanel();
            return PartialView("~/Views/Admin/_CategoriesPartial.cshtml", response.Categories);
        }
    }
}
