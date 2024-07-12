using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.OrserServices.PaymentService
{
    public class PaymentService : BaseService<Payment>, IPaymentService
    {
        public PaymentService(IBaseRepository<Payment> baseRepository) : base(baseRepository)
        {
        }
    }
}
