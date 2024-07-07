using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.ProductServices.ProductCategoryService
{
    public class ProductCategoryService : BaseService<ProductCategory>, IProductCategoryService
    {
        private readonly IMapper _mapper;

        public ProductCategoryService(IBaseRepository<ProductCategory> productCategoryRepository,
            IMapper mapper) : base(productCategoryRepository)
        {
            _mapper = mapper;
        }

        public async Task<ProductCategory> CreateCategory(RequestProductCategoryDto productCategory)
        {
            if (await IfExist(productCategory.Name.ToLower(), x => x.Name.ToLower()))
                throw new Exception("Category already exists");

            var reasponse = _mapper.Map<ProductCategory>(productCategory);
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
        }

        private async Task<bool> IfExist<TField>(TField field, Expression<Func<ProductCategory, TField>> selector)
        {
            var categoryIds = GetAll().AsQueryable()
                .Select(selector);

            return await categoryIds.ContainsAsync(field);
        }
    }
}
