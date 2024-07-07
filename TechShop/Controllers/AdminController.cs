using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AdminService;

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
        public IActionResult GetUsersAdminPanel()
        {
            var response = adminService.GetAdminPanel().Users;
            return PartialView("~/Views/Admin/_UsersPartial.cshtml", response);
        }

        [HttpGet]
        public IActionResult GetProductsAdminPanel()
        {
            var response = adminService.GetAdminPanel().Products;
            return PartialView("~/Views/Admin/_ProductsPartial.cshtml", response);
        }

        [HttpGet]
        public IActionResult GetCategoriesAdminPanel()
        {
            var response = adminService.GetAdminPanel().Categories;
            return PartialView("~/Views/Admin/_CategoriesPartial.cshtml", response);
        }
    }
}
