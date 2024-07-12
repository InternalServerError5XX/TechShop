using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Net.Http;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.StripeDto;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.OrserServices.PaymentService
{
    public class PaymentService : BaseService<Payment>, IPaymentService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<StripeSettings> _stripeSettings;

        public PaymentService(IBaseRepository<Payment> paymentRepository, IOptions<StripeSettings> stripeSettings, 
            IHttpContextAccessor httpContextAccessor) : base(paymentRepository)
        {
            _stripeSettings = stripeSettings;
            StripeConfiguration.ApiKey = _stripeSettings.Value.SecretKey;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<Session> CreatePaymentIntent(Basket basket)
        {
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in basket.BasketItems)
            {
                var productPhoto = item.Product.ProductPhotos.FirstOrDefault()?.Path;
                var imageUrl = productPhoto != null ? new List<string> 
                { $"{_contextAccessor.HttpContext.Request.Scheme}://" +
                    $"{_contextAccessor.HttpContext.Request.Host}/{productPhoto}" 
                } : null;

                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"{item.Product.Brand} {item.Product.Model}",
                            Images = imageUrl
                        },
                    },
                    Quantity = item.Quantity,
                };

                lineItems.Add(lineItem);
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = _stripeSettings.Value.SuccessUrl,
                CancelUrl = _stripeSettings.Value.CancelUrl,
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }

    }
}
