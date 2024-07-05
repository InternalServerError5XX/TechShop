using TechShop.Domain.Entities.ProductEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.ProductRepositories.ProductRepositoty
{
    public class ProductRepositoty : BaseRepository<Product>, IProductRepositoty
    {
        public ProductRepositoty(ApplicationDbContext context) : base(context)
        {
        }
    }
}
