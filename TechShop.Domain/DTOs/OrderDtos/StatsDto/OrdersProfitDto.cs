namespace TechShop.Domain.DTOs.OrderDtos.StatsDto
{
    public class OrdersProfitDto
    {
        public Dictionary<DateTime, decimal> MonthlyProfits { get; set; } = [];
        public decimal AverageProfit { get; set; }
    }
}
