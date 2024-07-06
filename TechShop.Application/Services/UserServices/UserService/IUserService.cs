using Microsoft.AspNetCore.Identity;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.Entities;

namespace TechShop.Application.Services.UserServices.UserService
{
    public interface IUserService
    {
        Task<string> GetUserId(string email);
        Task<IEnumerable<IdentityRole>> GetRoles();
        IQueryable<ApplicationUser> GetUsers();
        Task<UserProfile> GetUserProfile(string email);
        Task UpdateProfile(UserProfile profile);
    }
}
