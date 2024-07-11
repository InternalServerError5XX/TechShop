using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Enums;

namespace TechShop.Domain.DTOs.OrderDtos.PaymentDto
{
    public class ResponsePaymentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ResponseOrderDto Order { get; set; } = null!;
    }
}
