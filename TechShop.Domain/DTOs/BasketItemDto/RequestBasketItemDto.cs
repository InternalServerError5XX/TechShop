namespace TechShop.Domain.DTOs.BasketItemDto
{
    public class RequestBasketItemDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
