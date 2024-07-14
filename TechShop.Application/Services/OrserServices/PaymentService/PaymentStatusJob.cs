using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.OrserServices.OrserService;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Enums;

namespace TechShop.Application.Services.OrserServices.PaymentService
{
    public class PaymentStatusJob
    {
        private readonly IOrderService _orderService;
        private readonly IAdminService _adminService;

        public PaymentStatusJob(IOrderService orderService, IAdminService adminService)
        {
            _orderService = orderService;
            _adminService = adminService;
        }

        public async Task UpdatePaymentStatus()
        {
            var orders = _orderService.GetOrders();

            await _orderService.UpdateOrdersPaymentStatusTransaction(orders);
            _adminService.RemoveCachedAdminPanel();
        }
    }
}
