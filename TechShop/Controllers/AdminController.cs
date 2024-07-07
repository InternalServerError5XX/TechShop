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
            var response = adminService.GetAdminPanel();
            return View(response);
        }
    }
}
