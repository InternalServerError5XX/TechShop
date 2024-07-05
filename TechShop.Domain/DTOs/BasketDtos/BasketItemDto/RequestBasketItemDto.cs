namespace TechShop.Domain.DTOs.BasketDtos.BasketItemDto
{
    public class RequestBasketItemDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
