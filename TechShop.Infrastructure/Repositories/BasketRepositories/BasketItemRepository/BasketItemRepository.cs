using TechShop.Domain.Entities.BasketEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.BasketRepositories.BasketItemRepository
{
    public class BasketItemRepository : BaseRepository<BasketItem>, IBasketItemRepository
    {
        public BasketItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
