using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.BaseService;
using TechShop.Application.Services.ProductServices.ProductPhotoService;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.ProductServices.ProductService
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductPhotoService _productPhotoService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public ProductService(IBaseRepository<Product> productRepository, IProductPhotoService productPhotoService,
            IServiceProvider serviceProvider, IMapper mapper) : base(productRepository)
        {
            _productPhotoService = productPhotoService;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        private void RemoveAdminChache()
        {
            var adminChacheService = _serviceProvider.GetService<IAdminService>();
            if (adminChacheService == null)
                throw new Exception("Couldn't start Admin Service");

            adminChacheService.RemoveCachedAdminPanel();
        }

        public async Task<Product> CreateProduct(CreateProductDto productDto)
        {
            if (!productDto.ProductPhotos.Any())
                throw new NullReferenceException("Product photos are required.");

            await BeginTransactionAsync();

            try
            {
                var requestProduct = _mapper.Map<RequestProductDto>(productDto);
                var product = _mapper.Map<Product>(requestProduct);

                var productResponse = await AddAsync(product);
                if (productResponse == null)
                    throw new Exception("Couldn't create the product");

                foreach (var photoDto in requestProduct.ProductPhotos)
                    photoDto.ProductId = productResponse.Id;

                var savedPhotos = await _productPhotoService.SavePhotoAsync(requestProduct.ProductPhotos);
                if (!savedPhotos.Any())
                    throw new Exception("Couldn't create the photos");

                await CommitTransactionAsync();

                RemoveAdminChache();
                return productResponse;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task UpdateProduct(int id, CreateProductDto productDto)
        {
            await BeginTransactionAsync();

            try
            {
                var productCheck = GetAll().Where(x => x.Id == id).Select(x => x.Id);
                if (productCheck == null)
                    throw new NullReferenceException("Product not found");

                var productPhotos = await _productPhotoService.GetAll().Where(x => x.ProductId == id).ToListAsync();               
                var requestProduct = _mapper.Map<RequestProductDto>(productDto);
                var product = _mapper.Map<Product>(requestProduct);
                product.Id = id;

                if (!productDto.ProductPhotos.Any())
                    product.ProductPhotos = productPhotos;
                else if (productPhotos.Count() == productDto.ProductPhotos.Count())
                    product.ProductPhotos = (await _productPhotoService
                        .UpdatePhotoSameCount(requestProduct.ProductPhotos, productPhotos)).ToList();
                else
                    product.ProductPhotos = (await _productPhotoService
                        .UpdatePhoto(requestProduct.ProductPhotos, productPhotos, id)).ToList();           

                await UpdateAsync(product);

                await CommitTransactionAsync();
                RemoveAdminChache();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
            }
        }

        public async Task DeleteProduct(int id)
        {
            await BeginTransactionAsync();

            try
            {
                var product = await GetByIdAsync(id, x => x.ProductPhotos);
                if (product == null)
                    throw new NullReferenceException("Product not found");
               
                _productPhotoService.DeletePhotoFile(product.ProductPhotos);
                await DeleteAsync(product.Id);

                await CommitTransactionAsync();
                RemoveAdminChache();
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
            return GetAll()
                .Include(x => x.Category)
                .Include(x => x.ProductPhotos);
        }
    }
}
