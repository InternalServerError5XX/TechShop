using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Domain.Entities.UserEntities;
using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Configs;
using TechShop.Infrastructure.Configs.BasketConfigs;
using TechShop.Infrastructure.Configs.ProductConfigs;
using TechShop.Infrastructure.Configs.UserConfigs;
using TechShop.Infrastructure.Configs.WishlistConfigs;

namespace TechShop.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ShippingInfo> ShippingInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserProfileConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new ProductPhotoConfiguration());
            builder.ApplyConfiguration(new BasketConfiguration());
            builder.ApplyConfiguration(new BasketItemConfiguration());
            builder.ApplyConfiguration(new WishlistConfiguration());
            builder.ApplyConfiguration(new WishlistItemsConfiguration());
            builder.ApplyConfiguration(new ProductCategoryConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}
