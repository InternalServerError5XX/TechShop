using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.UserServices.UserProfileService;
using TechShop.Domain.DTOs.AuthDto;
using TechShop.Domain.DTOs.UserDtos.RoleDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.Entities.UserEntities;
using TechShop.Domain.Enums;
using TechShop.Domain.Identity;

namespace TechShop.Application.Services.UserServices.UserService
{
    public class UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IUserProfileService userProfileService, IServiceProvider serviceProvider, IMapper mapper) : IUserService
    {
        private void RemoveAdminChache()
        {
            var adminChacheService = serviceProvider.GetService<IAdminService>();
            if (adminChacheService == null)
                throw new Exception("Couldn't start Admin Service");

            adminChacheService.RemoveCachedAdminPanel();
        }

        public async Task<string> GetUserId(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NullReferenceException("User not found");

            return user.Id;
        }        

        public async Task<int> GetUsersCountByRole(string name)
        {
            var role = await roleManager.FindByNameAsync(name);
            if (role == null)
                throw new NullReferenceException("Role not found");

            var usersInRole = await userManager.GetUsersInRoleAsync(name);
            return usersInRole.Count();

        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            return userManager.Users
                    .Include(x => x.UserProfile)
                    .Include(x => x.Basket)
                        .ThenInclude(x => x.BasketItems)
                            .ThenInclude(x => x.Product)
                                .ThenInclude(p => p.ProductPhotos)
                    .Include(x => x.Basket)
                        .ThenInclude(x => x.BasketItems)
                            .ThenInclude(x => x.Product)
                                .ThenInclude(p => p.Category)
                    .Include(x => x.Wishlist)
                        .ThenInclude(x => x.WishlistItems)
                            .ThenInclude(wi => wi.Product)
                                .ThenInclude(p => p.ProductPhotos)
                    .Include(x => x.Wishlist)
                        .ThenInclude(x => x.WishlistItems)
                            .ThenInclude(wi => wi.Product)
                                .ThenInclude(p => p.Category)
                    .Include(x => x.Orders);
        }

        public async Task<ApplicationUser> GetUser(string id)
        {
            return await userManager.Users
                .Include(x => x.UserProfile)
                .SingleAsync(x => x.Id == id);
        }

        public async Task<ApplicationUser> CreateUser(RequestUserDto userDto)
        {
            var appUser = await userManager.FindByEmailAsync(userDto.User.Email);

            if (appUser != null)
                throw new Exception("User is already exists");

            var newUser = mapper.Map<ApplicationUser>(userDto.User);
            var response = await userManager.CreateAsync(newUser, userDto.User.Password);

            if (!response.Succeeded)
            {
                var errorsMessages =
                    response.Errors.Aggregate("", (current, error) => current + " " + error.Description);

                throw new Exception(errorsMessages);
            }

            var role = await GetRole(userDto.RoleId);
            await userManager.AddToRoleAsync(newUser, role.Name!);

            var profile = mapper.Map<UserProfile>(userDto.User);
            profile.UserId = newUser.Id;
            await userProfileService.UpdateAsync(profile);

            RemoveAdminChache();
            return await GetUser(newUser.Id);
        }

        public async Task UpdateUser(UpdateUserDto userDto)
        {
            var user = await GetUser(userDto.UserId);
            if (user == null)
                throw new NullReferenceException("User not found");

            var profile = mapper.Map<UserProfile>(userDto);
            profile.CreatedDate = user.UserProfile.CreatedDate;
            profile.LastLogin = user.UserProfile.LastLogin;
            await userProfileService.UpdateAsync(profile);

            var role = await GetRole(userDto.RoleId);
            if (role == null)
                throw new NullReferenceException("Role not found");

            var userRoles = await userManager.GetRolesAsync(user);
            var removeRolesResult = await userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeRolesResult.Succeeded)
            {
                var removeErrors = removeRolesResult.Errors.Aggregate("", (current, error) => current + " " + error.Description);
                throw new Exception(removeErrors);
            }

            var addRoleResult = await userManager.AddToRoleAsync(user, role.Name!);
            if (!addRoleResult.Succeeded)
            {
                var addErrors = addRoleResult.Errors.Aggregate("", (current, error) => current + " " + error.Description);
                throw new Exception(addErrors);
            }

            var response = await userManager.UpdateAsync(user);
            if (!response.Succeeded)
            {
                var errorsMessages =
                    response.Errors.Aggregate("", (current, error) => current + " " + error.Description);

                throw new Exception(errorsMessages);
            }

            RemoveAdminChache();
        }

        public async Task DeleteUser(string id)
        {
            var appUser = await GetUser(id);
            if (appUser == null)
                throw new Exception("User not found");

            var isAdmin = await userManager.IsInRoleAsync(appUser, "Admin");
            if (isAdmin)
                throw new Exception("Unable to delete admin account");

            var response = await userManager.DeleteAsync(appUser);

            if (!response.Succeeded)
            {
                var errorsMessages =
                    response.Errors.Aggregate("", (current, error) => current + " " + error.Description);

                throw new Exception(errorsMessages);
            }

            RemoveAdminChache();
        }

        public async Task<UserProfile> GetProfile(string userId)
        {
            return await userProfileService.GetAll()
                .SingleAsync(x => x.UserId == userId);
        }

        public async Task<UserProfile> GetUserProfile(string email)
        {
            var userId = await GetUserId(email);
            return await userProfileService.GetAll()
                .SingleAsync(x => x.UserId == userId);
        }

        public async Task UpdateProfile(UserProfile profile)
        {
            await userProfileService.UpdateAsync(profile);
            RemoveAdminChache();
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole> GetRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                throw new NullReferenceException("Role not found");

            return role;
        }

        public async Task<IdentityRole> GetRoleByUserId(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NullReferenceException("User not found");

            var roles = await userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
                throw new NullReferenceException("No roles found for the user");

            var roleName = roles.First();
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
                throw new NullReferenceException("Role not found");

            return role;
        }

        public async Task<IdentityRole> CreateRole(RequestRoleDto roleDto)
        {
            var role = new IdentityRole(roleDto.Name);

            var response = await roleManager.CreateAsync(role);
            if (!response.Succeeded)
                throw new Exception(response.Errors.First().Description);

            RemoveAdminChache();
            return role;
        }

        public async Task<IdentityRole> UpdateRole(IdentityRole identityRole)
        {
            var role = await roleManager.FindByIdAsync(identityRole.Id);
            if (role == null)
                throw new NullReferenceException("Role not found");

            role.Name = identityRole.Name;
            var response = await roleManager.UpdateAsync(role);
            if (!response.Succeeded)
                throw new Exception(response.Errors.First().Description);

            RemoveAdminChache();
            return role;
        }

        public async Task DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                throw new NullReferenceException("Role not found");

            var response = await roleManager.DeleteAsync(role);
            if (!response.Succeeded)
                throw new Exception(response.Errors.First().Description);

            RemoveAdminChache();
        }
    }
}
