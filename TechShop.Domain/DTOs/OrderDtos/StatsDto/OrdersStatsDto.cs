namespace TechShop.Domain.DTOs.OrderDtos.StatsDto
{
    public class OrdersStatsDto
    {
        public OrdersProfitDto OrdersProfit { get; set; } = null!;
        public OrderStatusDto OrderStatus { get; set; } = null!;
    }
}
