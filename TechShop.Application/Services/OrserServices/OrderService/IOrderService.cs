using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Enums;

namespace TechShop.Application.Services.OrserServices.OrserService
{
    public interface IOrderService : IBaseService<Order>
    {
        IQueryable<Order> GetOrders();
        IQueryable<Order> GetUsersOrders(string email);
        Task<Order> GetOrder(int id);
        Task<string> CreateOrder(string email, ShippingInfo shippingInfo);
        Task<Order> UpdateOrder(int id, OrderStatus orderStatus, ShippingInfo shippingInfo);
        Task UpdateOrdersPaymentStatusTransaction(IQueryable<Order> orders);
        Task<Order> UpdateOrdersPaymentStatusTransaction(Order order);
        Task<Order> CancelOrder(int id);
        Task DeleteOrder(int id);
    }
}
