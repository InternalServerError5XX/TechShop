using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Entities.ProductEntities;

namespace TechShop.Domain.DTOs.OrderDtos.OrderItemDto
{
    public class ResponseOrderItemDro
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public ResponseProductDto Product { get; set; } = null!;
    }
}
