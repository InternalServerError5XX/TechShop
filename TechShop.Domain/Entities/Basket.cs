﻿using TechShop.Domain.DTOs.UserDto;

namespace TechShop.Domain.Entities
{
    public class Basket : BaseEntity
    {
        public string UserId {  get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;
        public ICollection<BasketItem> BasketItems { get; set; } = null!;
    }
}