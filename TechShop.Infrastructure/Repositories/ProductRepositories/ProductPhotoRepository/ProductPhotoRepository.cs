using TechShop.Domain.Entities.ProductEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.ProductRepositories.ProductPhotoRepository
{
    public class ProductPhotoRepository : BaseRepository<ProductPhoto>, IProductPhotoRepository
    {
        public ProductPhotoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
