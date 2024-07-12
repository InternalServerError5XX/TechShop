using TechShop.Domain.Entities.OrderEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.OrderRepositories.OrderRepository
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
    }
}
