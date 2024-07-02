using TechShop.Domain.Entities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Infrastructure.Repositories.ProductRepositoty
{
    public class ProductRepositoty : BaseRepository<Product>, IProductRepositoty
    {
        public ProductRepositoty(ApplicationDbContext context) : base(context)
        {
        }
    }
}
