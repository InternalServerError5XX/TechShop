namespace TechShop.Domain.Entities
{
    public class WishlistItem : BaseEntity
    {
        public int WishlistId {  get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
        public Wishlist Wishlist { get; set; } = null!;
    }
}
