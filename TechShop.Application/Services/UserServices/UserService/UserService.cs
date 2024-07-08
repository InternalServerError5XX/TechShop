using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechShop.Application.Services.UserServices.UserProfileService;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.Entities;

namespace TechShop.Application.Services.UserServices.UserService
{
    public class UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IUserProfileService userProfileService) : IUserService
    {
        public async Task<string> GetUserId(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new NullReferenceException("User not found");

            return user.Id;
        }

        public async Task<IEnumerable<IdentityRole>> GetRoles()
        {
            return await roleManager.Roles.ToListAsync();
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
                                .ThenInclude(p => p.Category);
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
        }
    }
}
