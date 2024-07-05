using Microsoft.AspNetCore.Identity;
using TechShop.Domain.Entities;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.WishlistEntities;

namespace TechShop.Domain.DTOs.UserDtos.UserDto
{
    public class ApplicationUser : IdentityUser
    {
        public UserProfile UserProfile { get; set; } = null!;
        public Basket Basket { get; set; } = null!;
        public Wishlist Wishlist { get; set; } = null!;
    }
}
