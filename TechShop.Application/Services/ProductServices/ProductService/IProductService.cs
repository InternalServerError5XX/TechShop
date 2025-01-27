﻿using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.Entities.ProductEntities;

namespace TechShop.Application.Services.ProductServices.ProductService
{
    public interface IProductService : IBaseService<Product>
    {
        IQueryable<Product> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> CreateProduct(CreateProductDto productDto);
        Task UpdateProduct(int id, CreateProductDto productDto);
        Task DeleteProduct(int id);
    }
}
