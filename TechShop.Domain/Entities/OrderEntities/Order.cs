﻿using TechShop.Domain.Entities.UserEntities;
using TechShop.Domain.Enums;

namespace TechShop.Domain.Entities.OrderEntities
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public int ShippingInfoId { get; set; }
        public OrderStatus Status { get; set; }
      
        public ApplicationUser User { get; set; } = null!;
        public ShippingInfo ShippingInfo { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public Payment Payment { get; set; } = null!;
    }
}
