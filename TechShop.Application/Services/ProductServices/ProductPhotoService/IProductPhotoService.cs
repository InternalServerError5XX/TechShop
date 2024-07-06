using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductDtos.ProductPhoto;
using TechShop.Domain.Entities.ProductEntities;

namespace TechShop.Application.Services.ProductServices.ProductPhotoService
{
    public interface IProductPhotoService : IBaseService<ProductPhoto>
    {
        Task<IEnumerable<ProductPhoto>> SavePhotoAsync(IEnumerable<RequestProductPhotoDto> productPhotoDtos);
        Task<IEnumerable<ProductPhoto>> UpdatePhoto(IEnumerable<RequestProductPhotoDto> productPhotoDto, IEnumerable<ProductPhoto> productPhotos);
        void DeletePhotoFile(IEnumerable<ProductPhoto> productPhotos);
    }
}
