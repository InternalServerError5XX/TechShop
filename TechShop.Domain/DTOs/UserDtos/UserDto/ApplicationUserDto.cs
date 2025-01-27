﻿using Microsoft.AspNetCore.Identity;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.DTOs.UserDtos.UserProfileDto;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShop.Domain.DTOs.UserDtos.UserDto
{
    public class ApplicationUserDto : IdentityUser
    {
        public ResponseUserProfileDto UserProfile { get; set; } = null!;
        public ResponseBasketDto Basket { get; set; } = null!;
        public ResponseWishlistDto Wishlist { get; set; } = null!;
        public ICollection<ResponseOrderDto> Orders { get; set; } = [];
    }
}
