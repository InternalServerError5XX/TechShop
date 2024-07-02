using TechShop.Domain.Entities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.ProductPhotoRepository
{
    public class ProductPhotoRepository : BaseRepository<ProductPhoto>, IProductPhotoRepository
    {
        public ProductPhotoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
