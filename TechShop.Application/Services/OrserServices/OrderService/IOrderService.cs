using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShop.Application.Services.OrserServices.OrserService
{
    public interface IOrderService : IBaseService<Order>
    {
        IQueryable<Order> GetOrders();
        IQueryable<Order> GetUsersOrders(string email);
        Task<Order?> GetOrder(int id);
        Task<Order> CreateOrder(string email, Order order);
    }
}
