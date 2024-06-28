using Microsoft.AspNetCore.Mvc;

namespace TechShopWeb.Controllers
{
    public class ErrorController : Controller
    {
        public async Task<IActionResult> Error404()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> Error403()
        {
            return await Task.FromResult(View());
        }
    }
}
