using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.UserProfileService
{
    public class UserProfileService : BaseService<UserProfile>, IUserProfileService
    {
        public UserProfileService(IBaseRepository<UserProfile> profileRepository) : base(profileRepository)
        {
        }
    }
}
