namespace TechShop.Domain.Entities
{
    public class BasketItem : BaseEntity
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; } = null!;
        public Basket Basket { get; set; } = null!;
    }
}
