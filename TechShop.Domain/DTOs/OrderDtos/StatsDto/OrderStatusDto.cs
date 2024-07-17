using TechShop.Domain.Enums;

namespace TechShop.Domain.DTOs.OrderDtos.StatsDto
{
    public class OrderStatusDto
    {
        public int TotalOrders { get; set; }
        public Dictionary<OrderStatus, int> StatusCounts { get; set; } = [];
    }
}
