using System.ComponentModel.DataAnnotations;
using TechShop.Domain.Enums;

namespace TechShop.Domain.DTOs.OrderDtos.PaymentDto
{
    public class RequestPaymentDto
    {

        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Required]
        public decimal Amount { get; set; }
    }
}
