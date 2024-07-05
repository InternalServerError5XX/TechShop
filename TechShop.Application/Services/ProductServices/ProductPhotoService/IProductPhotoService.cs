using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductDtos.ProductPhoto;
using TechShop.Domain.Entities.ProductEntities;

namespace TechShop.Application.Services.ProductServices.ProductPhotoService
{
    public interface IProductPhotoService : IBaseService<ProductPhoto>
    {
        Task<IEnumerable<ProductPhoto>> SavePhoto(IEnumerable<RequestProductPhotoDto> productPhotoDtos);
        Task DeletePhoto(int id);
    }
}
