using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TechShop.Domain.Entities.BasketEntities;

namespace TechShop.Infrastructure.Configs
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(b => b.User)
                .WithOne(u => u.Basket)
                .HasForeignKey<Basket>(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.BasketItems)
                .WithOne(bi => bi.Basket)
                .HasForeignKey(bi => bi.BasketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
