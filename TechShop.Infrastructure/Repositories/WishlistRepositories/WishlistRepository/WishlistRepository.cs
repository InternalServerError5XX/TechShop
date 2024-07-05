using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.WishlistRepositories.WishlistRepository
{
    public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
