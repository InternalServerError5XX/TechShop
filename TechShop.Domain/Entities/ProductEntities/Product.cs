using TechShop.Domain.Enums;

namespace TechShop.Domain.Entities.ProductEntities
{
    public class Product : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public ProductCategory Category { get; set; } = null!;
        public ICollection<ProductPhoto> ProductPhotos { get; set; } = [];
    }
}
