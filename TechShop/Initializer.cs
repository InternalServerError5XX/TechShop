using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using TechShop.Application.Extensions;
using TechShop.Application.Services.AuthService;
using TechShop.Application.Services.BasketServices.BasketItemService;
using TechShop.Application.Services.BasketServices.BasketService;
using TechShop.Application.Services.ProductServices.ProductCategoryService;
using TechShop.Application.Services.ProductServices.ProductPhotoService;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Application.Services.TempDataService;
using TechShop.Application.Services.UserServices.UserProfileService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Application.Services.WishlistServices.WishlistItemService;
using TechShop.Application.Services.WishlistServices.WishlistService;
using TechShop.Domain.DTOs.JWTDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Infrastructure;
using TechShop.Infrastructure.Repositories.BaseRepository;
using TechShop.Infrastructure.Repositories.BasketRepositories.BasketItemRepository;
using TechShop.Infrastructure.Repositories.BasketRepositories.BasketRepository;
using TechShop.Infrastructure.Repositories.ProductRepositories.ProductCategoryRepository;
using TechShop.Infrastructure.Repositories.ProductRepositories.ProductPhotoRepository;
using TechShop.Infrastructure.Repositories.ProductRepositories.ProductRepositoty;
using TechShop.Infrastructure.Repositories.UserProfileRepository;
using TechShop.Infrastructure.Repositories.WishlistRepositories.WishlistItemRepository;
using TechShop.Infrastructure.Repositories.WishlistRepositories.WishlistRepository;
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
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<IWishlistItemRepository, WishlistItemRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketItemRepository, BasketItemRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {                  
            services.AddScoped<ITempDataService, TempDataService>();           
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductPhotoService, ProductPhotoService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IWishlistService, WishlistService>();
            services.AddScoped<IWishlistItemService, WishlistItemService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketItemService, BasketItemService>();
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

            services.AddScoped<RoleManager<IdentityRole>>();
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

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var groupName = apiDesc.GroupName ?? string.Empty;
                    return docName == groupName || docName == "v1";
                });

                c.TagActionsBy(apiDesc =>
                {
                    if (apiDesc.GroupName != null)
                    {
                        return new[] { apiDesc.GroupName };
                    }

                    return new[] { apiDesc.HttpMethod };
                });
            });
        }
    }
}
