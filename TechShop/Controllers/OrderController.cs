using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(CreateOrderDto createOrderDto)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var basket = await basketService.GetUserBasket(email!);
            var basketResponse = mapper.Map<ResponseBasketDto>(basket);
            createOrderDto.Basket = basketResponse;

            if (!ModelState.IsValid)
                return View(createOrderDto);

            var shippingInfo = mapper.Map<ShippingInfo>(createOrderDto.ShippingInfo);
            var response = await orderService.CreateOrder(email!, shippingInfo);

            return Redirect(response);
        }

        [HttpGet]
        public async Task<IActionResult> UserOrders()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var orders = orderService.GetUsersOrders(email!);
            await orderService.UpdateOrdersPaymentStatusTransaction(orders);
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
    }
}
