using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechShop.Application.Services.BasketServices.BasketService;
using TechShop.Application.Services.OrserServices.OrserService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.DTOs.OrderDtos.ShippingInfoDto;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShopWeb.Controllers
{
    [Authorize]
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class OrderController(IBasketService basketService, IOrderService orderService, IUserService userService,
        IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> OrderProducts()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var basket = await basketService.GetUserBasket(email!);
            var response = mapper.Map<ResponseBasketDto>(basket);

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmOrder()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profile = await userService.GetUserProfile(email!);
            var response = mapper.Map<RequestShippingInfoDto>(profile);

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(RequestShippingInfoDto shippingInfoDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();
                return BadRequest(new { Errors = errors });
            }

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var shippingInfo = mapper.Map<ShippingInfo>(shippingInfoDto);
            var response = await orderService.CreateOrder(email!, shippingInfo);

            var token = Request.Cookies["token"];
            Response.Cookies.Append("token", token!, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = false,
            });

            return Ok(new { RedirectUrl = response });
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await orderService.GetOrder(id);
            var response = mapper.Map<ResponseOrderDto>(order);

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> UserOrders()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var orders = orderService.GetUsersOrders(email!);
            var response = mapper.Map<IEnumerable<ResponseOrderDto>>(orders);

            return PartialView("~/Views/Shared/_UserOrdersModal.cshtml", response);
        }       

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            var order = await orderService.GetOrder(id);
            var response = mapper.Map<ResponseOrderDto>(order);

            return View(response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var shippingInfo = mapper.Map<ShippingInfo>(updateOrderDto.ShippingInfoDto);
            var order = await orderService.UpdateOrder(updateOrderDto.Id, updateOrderDto.OrderStatus, shippingInfo);

            var response = mapper.Map<ResponseOrderDto>(order);
            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await orderService.CancelOrder(id);
            var response = mapper.Map<ResponseOrderDto>(order);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await orderService.DeleteOrder(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> SuccessOrder()
        {
            var token = Request.Cookies["token"];
            Response.Cookies.Append("token", token!, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = false,
            });

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var order = await orderService.GetUsersOrders(email!)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            var response = mapper.Map<ResponseOrderDto>(order);
            return View(response);
        }

        [HttpGet]
        public IActionResult FailedOrder()
        {
            return View();
        }
    }
}
