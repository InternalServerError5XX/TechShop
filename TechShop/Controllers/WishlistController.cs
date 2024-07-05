using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TechShop.Application.Services.WishlistServices.WishlistService;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;

namespace TechShopWeb.Controllers
{
    [Authorize]
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class WishlistController(IWishlistService wishlistService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Add([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var reffer = Request.Headers["Referer"].ToString();
            await wishlistService.AddToWishlist(email!, productId);

            return Redirect(reffer);
        }

        public async Task<IActionResult> DeleteByProductId([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var reffer = Request.Headers["Referer"].ToString();
            await wishlistService.DeleteFromWishlistByProductId(email!, productId);

            return Redirect(reffer);
        }

        public async Task<IActionResult> DeleteById([Required] int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var reffer = Request.Headers["Referer"].ToString();
            await wishlistService.DeleteFromWishlistById(email!, id);

            return Redirect(reffer);
        }

        public async Task<IActionResult> Get()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var wishlists = await wishlistService.GetUserWishlist(email!);
            var response = mapper.Map<ResponseWishlistDto>(wishlists);

            return PartialView("~/Views/Shared/_WishlistModal.cshtml", response);
        }
    }
}
