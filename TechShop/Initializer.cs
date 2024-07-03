using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using TechShop.Application.Extensions;
using TechShop.Application.Services.AuthService;
using TechShop.Application.Services.ProductPhotoService;
using TechShop.Application.Services.ProductService;
using TechShop.Application.Services.TempDataService;
using TechShop.Application.Services.UserProfileService;
using TechShop.Application.Services.UserService;
using TechShop.Domain.DTOs.JWTDto;
using TechShop.Domain.DTOs.UserDto;
using TechShop.Infrastructure;
using TechShop.Infrastructure.Repositories.BaseRepository;
using TechShop.Infrastructure.Repositories.ProductPhotoRepository;
using TechShop.Infrastructure.Repositories.ProductRepositoty;
using TechShop.Infrastructure.Repositories.UserProfileRepository;
using TechShopWeb.Filters;

namespace TechShop
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IProductRepositoty, ProductRepositoty>();
            services.AddScoped<IProductPhotoRepository, ProductPhotoRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {                  
            services.AddScoped<ITempDataService, TempDataService>();           
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductPhotoService, ProductPhotoService>();
        }

        public static void InitializeFilters(this IServiceCollection services)
        {
            services.AddScoped<MvcControllerExceptionFilter>();
            services.AddScoped<ApiControllerExceptionFilter>();
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
