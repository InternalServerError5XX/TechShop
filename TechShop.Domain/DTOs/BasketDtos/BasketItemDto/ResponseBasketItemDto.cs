using TechShop.Domain.DTOs.ProductDtos.ProductDto;

namespace TechShop.Domain.DTOs.BasketDtos.BasketItemDto
{
    public class ResponseBasketItemDto
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ResponseProductDto Product { get; set; } = null!;
    }
}
