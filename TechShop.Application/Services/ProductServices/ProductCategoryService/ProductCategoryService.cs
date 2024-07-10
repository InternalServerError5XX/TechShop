using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.ProductServices.ProductCategoryService
{
    public class ProductCategoryService : BaseService<ProductCategory>, IProductCategoryService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;        

        public ProductCategoryService(IBaseRepository<ProductCategory> productCategoryRepository,
            IServiceProvider serviceProvider, IMapper mapper) : base(productCategoryRepository)
        {
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

        public async Task<ProductCategory> CreateCategory(RequestProductCategoryDto productCategory)
        {
            if (await IfExist(productCategory.Name.ToLower(), x => x.Name.ToLower()))
                throw new Exception("Category already exists");

            var reasponse = _mapper.Map<ProductCategory>(productCategory);

            RemoveAdminChache();
            return await AddAsync(reasponse);
        }

        public async Task UpdateCategory(int id, RequestProductCategoryDto productCategory)
        {
            var categoryCheck = await GetByIdAsync(id);
            if (categoryCheck == null)
                throw new NullReferenceException("Category not found");

            var category = _mapper.Map<ProductCategory>(productCategory);
            category.Id = id;
            category.CreatedDate = categoryCheck.CreatedDate;

            await UpdateAsync(category);
            RemoveAdminChache();
        }

        public async Task DeleteCategory(int id)
        {
            await DeleteAsync(id);
            RemoveAdminChache();
        }

        private async Task<bool> IfExist<TField>(TField field, Expression<Func<ProductCategory, TField>> selector)
        {
            var categoryIds = GetAll().AsQueryable()
                .Select(selector);

            return await categoryIds.ContainsAsync(field);
        }
    }
}
