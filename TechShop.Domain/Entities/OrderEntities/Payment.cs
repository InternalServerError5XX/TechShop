using TechShop.Domain.Enums;

namespace TechShop.Domain.Entities.OrderEntities
{
    public class Payment : BaseEntity
    {
        public string SessionId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public Order Order { get; set; } = null!;
    }
}
