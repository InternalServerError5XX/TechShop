using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShop.Domain.DTOs.OrderDtos.ShippingInfoDto
{
    public class ResponseShippingInfoDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
