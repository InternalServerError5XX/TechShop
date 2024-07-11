using TechShop.Domain.DTOs.AdminDto;
using TechShop.Domain.DTOs.OrderDtos.OrderItemDto;
using TechShop.Domain.DTOs.OrderDtos.ShippingInfoDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Entities.UserEntities;
using TechShop.Domain.Enums;

namespace TechShop.Domain.DTOs.OrderDtos.OrderDto
{
    public class ResponseOrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ShippingInfoId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ApplicationUserDto User { get; set; } = null!;
        public ResponseShippingInfoDto ShippingInfo { get; set; } = null!;
        public ICollection<ResponseOrderItemDro> OrderItems { get; set; } = [];
        public ResponseAdminDto Payment { get; set; } = null!;
    }
}
