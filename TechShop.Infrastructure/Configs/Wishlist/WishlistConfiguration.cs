using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Domain.Entities.WishlistEntities;

namespace TechShop.Infrastructure.Configs.Wishlist
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(w => w.User)
                .WithOne(u => u.Wishlist)
                .HasForeignKey<Wishlist>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(w => w.WishlistItems)
                .WithOne(wi => wi.Wishlist)
                .HasForeignKey(wi => wi.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
