using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Net.Http;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.StripeDto;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Enums;
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
            var baseUrl = $"{_contextAccessor.HttpContext!.Request.Scheme}://" +
                    $"{_contextAccessor.HttpContext.Request.Host}";

            foreach (var item in basket.BasketItems)
            {
                var productPhoto = item.Product.ProductPhotos.FirstOrDefault()?.Path;
                var imageUrl = new List<string> { $"{baseUrl}/{productPhoto}" };

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
                SuccessUrl = $"{baseUrl}/{_stripeSettings.Value.SuccessUrl}",
                CancelUrl = $"{baseUrl}/{_stripeSettings.Value.CancelUrl}",
                ExpiresAt = DateTime.Now.AddHours(24)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }

        public async Task<Payment> UpdatePayment(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
                throw new NullReferenceException("Payment not found");

            var stripeResponse = await GetStripeSession(payment.SessionId);

            if (stripeResponse.Status == "complete" && stripeResponse.PaymentStatus == "paid")
                payment.Status = PaymentStatus.Completed;
            else
                payment.Status = PaymentStatus.Failed;

            await UpdateAsync(payment);
            return payment;
        }

        public async Task<Payment> UpdatePayment(Order order)
        {
            var payment = order.Payment;
            order.User = null!;
            if (payment == null)
                throw new NullReferenceException("Payment not found");

            var stripeResponse = await GetStripeSession(payment.SessionId);

            if (stripeResponse.Status == "complete" && stripeResponse.PaymentStatus == "paid")
            {
                payment.Status = PaymentStatus.Completed;
                await UpdateAsync(payment);
            }
                
            else if(order.CreatedDate > DateTime.Now.AddDays(1))
            {
                payment.Status = PaymentStatus.Failed;
                await UpdateAsync(payment);
            }

            return payment;
        }

        public async Task<IEnumerable<Payment>> UpdatePayment(IEnumerable<Order> orders)
        {
            var payments = new List<Payment>();

            foreach (var order in orders)
            {
                var payment = order.Payment;
                order.OrderItems = null!;
                order.User = null!;
                if (payment == null)
                    throw new NullReferenceException("Payment not found");

                var stripeResponse = await GetStripeSession(payment.SessionId);

                if (stripeResponse.Status == "complete" && stripeResponse.PaymentStatus == "paid" && 
                    payment.Status == PaymentStatus.Pending)
                {
                    payment.Status = PaymentStatus.Completed;
                    payments.Add(payment);
                }
                else if (DateTime.Now > order.CreatedDate.AddDays(1))
                {
                    payment.Status = PaymentStatus.Failed;
                    payments.Add(payment);
                }                
            }

            await UpdateRangeAsync(payments);
            return payments;
        }

        public async Task<Session> GetStripeSession(string sessionId)
        {
            var service = new SessionService();
            var response = await service.GetAsync(sessionId);
            if (response == null)
                throw new NullReferenceException("Stripe payment not found");

            return response;
        }

        public async Task RefundPayment(Order order)
        {
            var payment = await GetStripeSession(order.Payment.SessionId);

            if (payment.Status != "complete" || payment.PaymentStatus != "paid")
                throw new Exception("Payment has not been successfull");

            var refundAmount = payment.AmountTotal;

            var refundService = new RefundService();
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = payment.PaymentIntentId,
                Amount = refundAmount,
                Reason = "requested_by_customer",
            };

            var refund = await refundService.CreateAsync(refundOptions);

            if (refund.Status != "succeeded")
                throw new Exception("Refund failed");
        }
    }
}
