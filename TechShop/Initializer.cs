using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using TechShop.Application.Extensions;
using TechShop.Application.Services.AuthService;
using TechShop.Domain.DTOs.JWT;
using TechShop.Domain.DTOs.User;
using TechShop.Infrastructure;

namespace TechShop
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {

        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
        }

        public static void InitializeIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void InitializeAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JwtTokenSettings>(
                builder.Configuration.GetSection(nameof(JwtTokenSettings)));

            builder.Services.AddJwtAuthentication(
                builder.Configuration.GetValue<string>("JwtTokenSettings:JwtIssuer"),
                builder.Configuration.GetValue<string>("JwtTokenSettings:JwtAudience"),
                builder.Configuration.GetValue<string>("JwtTokenSettings:JwtKey"),
                builder.Configuration.GetValue<int>("JwtTokenSettings:JwtExpires")
            );
        }

        public static void InitializeSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Tech Shop",
                    Description = "Tech Shop API testing"
                });
            });
        }
    }
}
