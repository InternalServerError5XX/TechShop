using TechShop.Domain.DTOs.OrderDtos.ShippingInfoDto;
using TechShop.Domain.Enums;

namespace TechShop.Domain.DTOs.OrderDtos.OrderDto
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public RequestShippingInfoDto ShippingInfoDto { get; set; } = null!;
    }
}
