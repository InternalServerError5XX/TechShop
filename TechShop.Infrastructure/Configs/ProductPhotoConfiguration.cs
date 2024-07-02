using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechShop.Domain.Entities;

namespace TechShop.Infrastructure.Configs
{
    public class ProductPhotoConfiguration : IEntityTypeConfiguration<ProductPhoto>
    {
        public void Configure(EntityTypeBuilder<ProductPhoto> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPhotos)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
