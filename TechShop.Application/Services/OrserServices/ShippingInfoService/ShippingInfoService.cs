using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.OrserServices.ShippingInfoServie
{
    public class ShippingInfoService : BaseService<ShippingInfo>, IShippingInfoService
    {
        public ShippingInfoService(IBaseRepository<ShippingInfo> baseRepository) : base(baseRepository)
        {
        }
    }
}
