namespace TechShop.Domain.Entities
{
    public class ProductPhoto : BaseEntity
    {
        public int ProductId { get; set; }
        public string Path { get; set; } = string.Empty;

        public Product Product { get; set; } = null!;
    }
}
