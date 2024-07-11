using System.ComponentModel.DataAnnotations;
using TechShop.Domain.Enums;

namespace TechShop.Domain.DTOs.OrderDtos.OrderDto
{
    public class RequestOrderDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int ShippingInfoId { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
