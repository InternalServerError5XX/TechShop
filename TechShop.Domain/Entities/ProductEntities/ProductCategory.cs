﻿namespace TechShop.Domain.Entities.ProductEntities
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = null!;
    }
}
