using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.OrderDtos.ShippingInfoDto
{
    public class RequestShippingInfoDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string Street { get; set; } = string.Empty;
    }
}
