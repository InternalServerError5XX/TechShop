using TechShop.Domain.Entities.BasketEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.BasketRepositories.BasketRepository
{
    public class BasketRepository : BaseRepository<Basket>, IBasketRepository
    {
        public BasketRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
