using Microsoft.AspNetCore.Identity;
using TechShop.Domain.Entities;

namespace TechShop.Domain.DTOs.UserDto
{
    public class ApplicationUser : IdentityUser
    {
        public UserProfile UserProfile { get; set; } = null!;
        public Basket Basket { get; set; } = null!;
        public Wishlist Wishlist { get; set; } = null!;
    }
}
