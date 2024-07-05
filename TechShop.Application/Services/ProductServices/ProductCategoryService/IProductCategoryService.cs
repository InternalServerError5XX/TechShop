using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.Entities.ProductEntities;

namespace TechShop.Domain.DTOs.ProductDtos.ProductCategoryService
{
    public interface IProductCategoryService : IBaseService<ProductCategory>
    {
        Task<ProductCategory> CreateCategory(RequestProductCategoryDto productCategory);
        Task<bool> IsInWishlist<TField>(TField field, Expression<Func<ProductCategory, TField>> selector);
    }
}
