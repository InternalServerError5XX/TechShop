using Microsoft.AspNetCore.Identity;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.Entities;

namespace TechShop.Application.Services.UserServices.UserService
{
    public interface IUserService
    {
        Task<string> GetUserId(string email);
        Task<IEnumerable<IdentityRole>> GetRoles();
        Task<int> GetUsersCountByRole(string name);
        IQueryable<ApplicationUser> GetUsers();
        Task<UserProfile> GetUserProfile(string email);
        Task UpdateProfile(UserProfile profile);
    }
}
