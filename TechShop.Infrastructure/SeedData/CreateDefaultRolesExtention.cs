using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TechShop.Domain.Identity;

namespace TechShop.Infrastructure.SeedData
{
    public static class CreateDefaultRolesExtention
    {
        public static async Task CreateDefaultRoles(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[]
            {
                DefaultRoles.Admin,
                DefaultRoles.User,
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
