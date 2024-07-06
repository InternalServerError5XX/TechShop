using AutoMapper;
using TechShop.Application.Services.BaseService;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Domain.DTOs.ProductDtos.ProductPhoto;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;
using TechShop.Infrastructure.Repositories.ProductRepositories.ProductRepositoty;

namespace TechShop.Application.Services.ProductServices.ProductPhotoService
{
    public class ProductPhotoService : BaseService<ProductPhoto>, IProductPhotoService
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<ProductPhoto> _productPhotoRepository;

        public ProductPhotoService(IBaseRepository<ProductPhoto> productPhotoRepository,
            IProductRepositoty productRepositoty, IMapper mapper) : base(productPhotoRepository)
        {
            _mapper = mapper;
            _productPhotoRepository = productPhotoRepository;
        }

        public async Task<IEnumerable<ProductPhoto>> SavePhotoAsync(IEnumerable<RequestProductPhotoDto> productPhotoDtos)
        {
            var savedPhotos = new List<ProductPhoto>();

            foreach (var productPhotoDto in productPhotoDtos)
            {
                var extension = CheckExtension(productPhotoDto);
                var fileName = Guid.NewGuid().ToString() + extension;
                var directoryPath = Path.Combine("wwwroot", "images", "products");
                var filePath = Path.Combine(directoryPath, fileName);

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await productPhotoDto.Photo.CopyToAsync(stream);

                var productPhoto = _mapper.Map<ProductPhoto>(productPhotoDto);
                productPhoto.Path = $"/images/products/{fileName}";

                savedPhotos.Add(productPhoto);
            }

            return await AddRangeAsync(savedPhotos);
        }

        public async Task<IEnumerable<ProductPhoto>> UpdatePhoto(IEnumerable<RequestProductPhotoDto> productPhotoDto,
            IEnumerable<ProductPhoto> productPhotos)
        {
            DeletePhoto(productPhotos);
            var photos = await SavePhoto(productPhotoDto);
            var date = DateTime.Now;

            for (int i = 0; i < productPhotos.Count(); i++)
            {
                var photo = photos.ElementAtOrDefault(i);
                productPhotos.ElementAt(i).Path = photo!.Path;
                productPhotos.ElementAt(i).UpdatedDate = date;
            }

            return productPhotos;
        }

        public void DeletePhotoFile(IEnumerable<ProductPhoto> productPhotos)
        {
            foreach (var photo in productPhotos)
            {
                var filePath = Path.Combine("wwwroot", photo.Path.TrimStart('/'));
                if (!File.Exists(filePath))
                    throw new NullReferenceException("Photo not found from the server");

                File.Delete(filePath);
            }
        }

        private string CheckExtension(RequestProductPhotoDto productPhotoDto)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var extension = Path.GetExtension(productPhotoDto.Photo.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Invalid file type. Only JPG, JPEG, PNG, WEBP, and GIF files are allowed.");

            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };
            if (!allowedContentTypes.Contains(productPhotoDto.Photo.ContentType.ToLowerInvariant()))
                throw new ArgumentException("Invalid file content type.");

            return extension;
        }

        private async Task<IEnumerable<ProductPhoto>> SavePhoto(IEnumerable<RequestProductPhotoDto> productPhotoDtos)
        {
            var savedPhotos = new List<ProductPhoto>();

            foreach (var productPhotoDto in productPhotoDtos)
            {
                var extension = CheckExtension(productPhotoDto);
                var fileName = Guid.NewGuid().ToString() + extension;
                var directoryPath = Path.Combine("wwwroot", "images", "products");
                var filePath = Path.Combine(directoryPath, fileName);

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await productPhotoDto.Photo.CopyToAsync(stream);

                var productPhoto = _mapper.Map<ProductPhoto>(productPhotoDto);
                productPhoto.Path = $"/images/products/{fileName}";

                savedPhotos.Add(productPhoto);
            }

            return savedPhotos;
        }

        public async void DeletePhoto(IEnumerable<ProductPhoto> productPhotos)
        {
            foreach (var photo in productPhotos)
            {
                var filePath = Path.Combine("wwwroot", photo.Path.TrimStart('/'));
                if (!File.Exists(filePath))
                    throw new NullReferenceException("Photo not found from the server");

                File.Delete(filePath);
                await _productPhotoRepository.DeleteRangeAsync(productPhotos);
            }
        }
    }
}
