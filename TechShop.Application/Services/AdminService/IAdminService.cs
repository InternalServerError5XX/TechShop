using TechShop.Domain.DTOs.AdminDto;
using TechShop.Domain.DTOs.OrderDtos.StatsDto;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShop.Application.Services.AdminService
{
    public interface IAdminService
    {
        Task<ResponseAdminDto> GetAdminPanel();
        Task<ResponseAdminDto> GetCachedAdminPanel();
        Task<OrdersStatsDto> GetOrdersStats(DateTime? startDate, DateTime? endDate);
        Task<OrdersStatsDto> GetOrdersStats(IQueryable<Order> orders, DateTime? startDate, DateTime? endDate);
        void RemoveCachedAdminPanel();
    }
}
