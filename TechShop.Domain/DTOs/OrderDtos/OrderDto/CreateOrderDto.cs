using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.OrderDtos.ShippingInfoDto;

namespace TechShop.Domain.DTOs.OrderDtos.OrderDto
{
    public class CreateOrderDto
    {
        public RequestShippingInfoDto ShippingInfo { get; set; } = null!;
        public ResponseBasketDto Basket { get; set; } = null!;
    }
}
