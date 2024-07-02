using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductPhoto;
using TechShop.Domain.Entities;

namespace TechShop.Application.Services.ProductPhotoService
{
    public interface IProductPhotoService : IBaseService<ProductPhoto>
    {
        Task<IEnumerable<ProductPhoto>> SavePhoto(IEnumerable<RequestProductPhotoDto> productPhotoDtos);
        Task DeletePhoto(int id);
    }
}
