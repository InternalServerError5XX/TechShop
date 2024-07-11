using TechShop.Domain.Entities.UserEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.UserProfileRepository
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
