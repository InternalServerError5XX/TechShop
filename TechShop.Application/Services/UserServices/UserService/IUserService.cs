using Microsoft.AspNetCore.Identity;
using TechShop.Domain.DTOs.UserDtos.RoleDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.Entities;

namespace TechShop.Application.Services.UserServices.UserService
{
    public interface IUserService
    {
        Task<string> GetUserId(string email);       
        Task<int> GetUsersCountByRole(string name);
        IQueryable<ApplicationUser> GetUsers();
        Task<ApplicationUser> GetUser(string id);
        Task<ApplicationUser> CreateUser(RequestUserDto userDto);
        Task UpdateUser(UpdateUserDto userDto);
        Task DeleteUser(string id);
        Task<UserProfile> GetProfile(string userId);
        Task<UserProfile> GetUserProfile(string email);
        Task<IEnumerable<IdentityRole>> GetRoles();
        Task<IdentityRole> GetRole(string id);
        Task<IdentityRole> GetRoleByUserId(string userId);
        Task UpdateProfile(UserProfile profile);
        Task<IdentityRole> CreateRole(RequestRoleDto roleDto);
        Task<IdentityRole> UpdateRole(IdentityRole identityRole);
        Task DeleteRole(string id);
    }
}
