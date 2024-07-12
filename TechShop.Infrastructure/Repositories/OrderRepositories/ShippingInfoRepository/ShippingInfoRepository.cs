using TechShop.Domain.Entities.OrderEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.OrderRepositories.ShippingInfoRepository
{
    public class ShippingInfoRepository : BaseRepository<ShippingInfo>, IShippingInfoRepository
    {
        public ShippingInfoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
