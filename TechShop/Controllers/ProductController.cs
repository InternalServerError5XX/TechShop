using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechShop.Application.Services.ProductService;
using TechShop.Application.Services.TempDataService;
using TechShop.Domain.DTOs.FilterDto;
using TechShop.Domain.DTOs.PaginationDto;
using TechShop.Domain.DTOs.ProductDto;
using TechShop.Domain.Entities;

namespace TechShopWeb.Controllers
{
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class ProductController(IProductService productService, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(RequestPaginationDto paginationDto, string? searchTerm,
            string? orderBy, bool? isAsc)
        {
            var filterDto = new RequestFilterDto<Product>
            {
                SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null :
                        x => x.Brand.ToLower().Contains(searchTerm.ToLower()) ||
                        x.Model.ToLower().Contains(searchTerm.ToLower()),

                OrderBy = orderBy switch
                {
                    "price_desc" => x => x.Price,
                    "price_asc" => x => x.Price,
                    "date_desc" => x => x.CreatedDate,
                    "date_asc" => x => x.CreatedDate,
                    _ => null
                },
            };

            if (filterDto.OrderBy != null)
            {
                if (orderBy!.EndsWith("_desc"))
                    filterDto.IsAsc = false;
                else if (orderBy.EndsWith("_asc"))
                    filterDto.IsAsc = true;
                else filterDto.IsAsc = true;
            }

            var query = productService.GetFilteredQuery(productService.GetProducts(), filterDto);
            var products = await productService.GetPaginated(query, paginationDto);
            var response = mapper.Map<ResponsePaginationDto<ResponseProductDto>>(products);

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var products = await productService.GetProduct(id);
            var response = mapper.Map<ResponseProductDto>(products);

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestProductDto productDto)
        {
            var product = mapper.Map<Product>(productDto);
            var response = await productService.AddAsync(product);          

            return View(await GetById(response.Id));
        }
    }
}
