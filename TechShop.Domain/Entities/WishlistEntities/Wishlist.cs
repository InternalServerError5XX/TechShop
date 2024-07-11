using TechShop.Domain.Entities.UserEntities;

namespace TechShop.Domain.Entities.WishlistEntities
{
    public class Wishlist : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;
        public ICollection<WishlistItem> WishlistItems { get; set; } = [];
    }
}
