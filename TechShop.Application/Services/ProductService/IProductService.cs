using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductDto;
using TechShop.Domain.Entities;

namespace TechShop.Application.Services.ProductService
{
    public interface IProductService : IBaseService<Product>
    {
        IQueryable<Product> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> CreateProduct(RequestProductDto productDto);
    }
}
