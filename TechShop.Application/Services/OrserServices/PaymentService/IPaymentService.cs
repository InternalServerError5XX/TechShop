using Stripe;
using Stripe.Checkout;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShop.Application.Services.OrserServices.PaymentService
{
    public interface IPaymentService : IBaseService<Payment>
    {
        Task<Session> CreatePaymentIntent(Basket basket);
    }
}
