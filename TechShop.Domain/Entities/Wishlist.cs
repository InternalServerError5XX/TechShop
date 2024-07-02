﻿using TechShop.Domain.DTOs.UserDto;

namespace TechShop.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;
        public ICollection<WishlistItem> WishlistItems { get; set; } = [];
    }
}