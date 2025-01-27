﻿using TechShop.Domain.Entities.UserEntities;

namespace TechShop.Domain.Entities.BasketEntities
{
    public class Basket : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;
        public ICollection<BasketItem> BasketItems { get; set; } = null!;
    }
}
