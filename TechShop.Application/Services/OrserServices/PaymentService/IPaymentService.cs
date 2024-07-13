using Stripe;
using Stripe.Checkout;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShop.Application.Services.OrserServices.PaymentService
{
    public interface IPaymentService : IBaseService<Payment>
    {
        Task<Session> GetStripeSession(string sessionId);
        Task<Session> CreatePaymentIntent(Basket basket);
        Task<Payment> UpdatePayment(int id);
        Task<IEnumerable<Payment>> UpdatePayment(IEnumerable<Order> orders);
        Task<Payment> UpdatePayment(Order order);
        Task RefundPayment(Order order);
    }
}
