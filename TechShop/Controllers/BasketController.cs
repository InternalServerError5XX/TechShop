using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TechShop.Application.Services.BasketServices.BasketService;
using TechShop.Application.Services.WishlistServices.WishlistService;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;

namespace TechShopWeb.Controllers
{
    [Authorize]
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class BasketController(IBasketService basketService, IMapper mapper) : Controller
    {
        public async Task<IActionResult> Add([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var reffer = Request.Headers["Referer"].ToString();
            await basketService.AddToBasket(email!, productId);

            return Redirect(reffer);
        }

        public async Task<IActionResult> DeleteById([Required] int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var reffer = Request.Headers["Referer"].ToString();
            await basketService.DeleteFromBasketByProductId(email!, id);

            return Redirect(reffer);
        }

        public async Task<IActionResult> DeleteByProductId([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var reffer = Request.Headers["Referer"].ToString();
            await basketService.DeleteFromBasketByProductId(email!, productId);

            return Redirect(reffer);
        }

        public async Task<IActionResult> Get()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var wishlists = await basketService.GetUserBasket(email!);
            var response = mapper.Map<ResponseBasketDto>(wishlists);

            return PartialView("~/Views/Shared/_BasketModal.cshtml", response);
        }

        public async Task<IActionResult> Plus(int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            await basketService.EncreaseQuantityById(email!, id);

            return Ok();
        }

        public async Task<IActionResult> Minus(int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            await basketService.DecreaseQuantityById(email!, id);

            return Ok();
        }
    }
}
