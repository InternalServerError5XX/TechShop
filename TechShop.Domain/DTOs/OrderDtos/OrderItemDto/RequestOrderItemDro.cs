using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.OrderDtos.OrderItemDto
{
    public class RequestOrderItemDro
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
