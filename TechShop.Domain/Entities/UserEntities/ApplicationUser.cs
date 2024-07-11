using Microsoft.AspNetCore.Identity;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Entities.WishlistEntities;

namespace TechShop.Domain.Entities.UserEntities
{
    public class ApplicationUser : IdentityUser
    {
        public UserProfile UserProfile { get; set; } = null!;
        public Basket Basket { get; set; } = null!;
        public Wishlist Wishlist { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = [];
    }
}
