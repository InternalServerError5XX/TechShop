using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductPhoto;
using TechShop.Domain.Entities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.ProductPhotoService
{
    public class ProductPhotoService : BaseService<ProductPhoto>, IProductPhotoService
    {
        private readonly IMapper _mapper;

        public ProductPhotoService(IBaseRepository<ProductPhoto> productPhotoRepository,
            IMapper mapper) : base(productPhotoRepository)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductPhoto>> SavePhoto(IEnumerable<RequestProductPhotoDto> productPhotoDtos)
        {
            var savedPhotos = new List<ProductPhoto>();

            foreach (var productPhotoDto in productPhotoDtos)
            {
                await CheckProduct(productPhotoDto);
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

        public async Task DeletePhoto(int id)
        {
            await BeginTransactionAsync();

            try
            {
                var photo = await GetByIdAsync(id);
                if (photo == null)
                    throw new NullReferenceException("Photo not found from the DB");

                var filePath = Path.Combine("wwwroot", photo.Path.TrimStart('/'));
                if (!File.Exists(filePath))
                    throw new NullReferenceException("Photo not found from the server");

                File.Delete(filePath);
                await DeleteAsync(id);

                await CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        private async Task CheckProduct(RequestProductPhotoDto productPhotoDto)
        {
            if (productPhotoDto.ProductId == null)
                throw new NullReferenceException("Product id is required");

            var product = await GetByIdAsync(productPhotoDto.ProductId.Value);
            if (product == null)
                throw new NullReferenceException("Product not found");
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
    }
}
