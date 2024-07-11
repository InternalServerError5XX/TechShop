using TechShop.Domain.Entities.ProductEntities;

namespace TechShop.Domain.Entities.OrderEntities
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }     
        public int ProductId { get; set; }       
        public int Quantity { get; set; }
        public decimal Price { get; set; }


        public Product Product { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}
