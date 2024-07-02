using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TechShop.Application.Services.BaseService;
using TechShop.Application.Services.ProductPhotoService;
using TechShop.Domain.DTOs.ProductDto;
using TechShop.Domain.Entities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.ProductService
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductPhotoService _productPhotoService;
        private readonly IMapper _mapper;

        public ProductService(IBaseRepository<Product> productRepository, IProductPhotoService productPhotoService,
            IMapper mapper) : base(productRepository)
        {
            _productPhotoService = productPhotoService;
            _mapper = mapper;
        }

        public async Task<Product> CreateProduct(RequestProductDto productDto)
        {
            await BeginTransactionAsync();

            try
            {
                var product = _mapper.Map<Product>(productDto);

                var productResponse = await AddAsync(product);
                if (productResponse == null)
                    throw new Exception("Couldn't create the product");

                foreach (var photoDto in productDto.ProductPhotos)
                    photoDto.ProductId = productResponse.Id;

                var savedPhotos = await _productPhotoService.SavePhoto(productDto.ProductPhotos);
                if (!savedPhotos.Any())
                    throw new Exception("Couldn't create one of the photos");

                await CommitTransactionAsync();
                return productResponse;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            var response = await GetByIdAsync(id,
                    p => p.Category,
                    p => p.ProductPhotos);

            if (response == null)
                throw new NullReferenceException("Product not found");

            return response;
        }

        public IQueryable<Product> GetProducts()
        {
            return GetAllAsync()
                .Include(x => x.Category)
                .Include(x => x.ProductPhotos);
        }
    }
}
