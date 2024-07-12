using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.OrserServices.OrderItemService
{
    public class OrderItemService : BaseService<OrderItem>, IOrderItemService
    {
        public OrderItemService(IBaseRepository<OrderItem> orderItemRepository) : base(orderItemRepository)
        {
        }
    }
}
