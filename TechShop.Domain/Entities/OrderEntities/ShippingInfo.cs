namespace TechShop.Domain.Entities.OrderEntities
{
    public class ShippingInfo : BaseEntity
    {
        public int OrderId { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;

        public Order Order { get; set; } = null!;
    }
}
