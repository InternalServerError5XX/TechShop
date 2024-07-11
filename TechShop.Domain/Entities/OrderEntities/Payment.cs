using TechShop.Domain.Enums;

namespace TechShop.Domain.Entities.OrderEntities
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }       
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }

        public Order Order { get; set; } = null!;
    }
}
