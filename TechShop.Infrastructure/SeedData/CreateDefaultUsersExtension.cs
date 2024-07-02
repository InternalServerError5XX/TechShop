using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TechShop.Domain.DTOs.UserDto;
using TechShop.Domain.Identity;

namespace TechShop.Infrastructure.SeedData
{
    public static class CreateDefaultUsersExtension
    {
        public static async Task CreateDefaultUsers(this WebApplication app)
        {
            var scope = app.Services.CreateAsyncScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminUser = await userManager.FindByEmailAsync(DefaultUsers.AdminEmail);
            var normalUser = await userManager.FindByEmailAsync(DefaultUsers.UserEmail);

            if (adminUser == null && normalUser == null)
            {
                var admin = new ApplicationUser
                {
                    Email = DefaultUsers.AdminEmail,
                    UserName = DefaultUsers.AdminName,
                };

                var user = new ApplicationUser
                {
                    Email = DefaultUsers.UserEmail,
                    UserName = DefaultUsers.UserName,
                };

                await userManager.CreateAsync(admin, DefaultUsers.AdminPassword);
                await userManager.AddToRoleAsync(admin, DefaultRoles.Admin);

                await userManager.CreateAsync(user, DefaultUsers.UserPassword);
                await userManager.AddToRoleAsync(user, DefaultRoles.User);
            }
        }
    }
}
