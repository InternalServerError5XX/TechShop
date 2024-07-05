using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.WishlistRepositories.WishlistItemRepository
{
    public class WishlistItemRepository : BaseRepository<WishlistItem>, IWishlistItemRepository
    {
        public WishlistItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
