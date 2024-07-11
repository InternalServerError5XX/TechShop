using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.UserEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.UserServices.UserProfileService
{
    public class UserProfileService : BaseService<UserProfile>, IUserProfileService
    {
        public UserProfileService(IBaseRepository<UserProfile> profileRepository) : base(profileRepository)
        {
        }
    }
}
