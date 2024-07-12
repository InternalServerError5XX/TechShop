using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AppServices.CacheService;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Domain.DTOs.FilterDto;
using TechShop.Domain.DTOs.PaginationDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.Entities.ProductEntities;

namespace TechShopWeb.Controllers
{
    [TypeFilter(typeof(MvcControllerExceptionFilter))]
    public class ProductController(IProductService productService, IProductCategoryService productCategoryService, 
        IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(RequestPaginationDto paginationDto, string? searchTerm, string? orderBy)
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return View(productDto);

            var response = await productService.CreateProduct(productDto);
            return RedirectToAction(nameof(GetById), new { id = response.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await productService.GetByIdAsync(id);
            var response = mapper.Map<CreateProductDto>(product);
            return View(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return View(productDto);
            await productService.UpdateProduct(id, productDto);

            return RedirectToAction(nameof(GetById), new { id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await productCategoryService.GetByIdAsync(id);
            var response = mapper.Map<ResponseProductCaregoryDto>(category);

            return PartialView("~/Views/Product/Category/_GetCategoryModal.cshtml", response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(ResponseProductCaregoryDto caregoryDto)
        {
            var response = mapper.Map<ProductCategory>(caregoryDto);
            await productCategoryService.UpdateAsync(response);

            return Json(new { success = true });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCategory()
        {
            return PartialView("~/Views/Product/Category/_CreateCategoryModal.cshtml", new RequestProductCategoryDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory(RequestProductCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return PartialView("~/Views/Product/Category/_CreateCategoryModal.cshtml", categoryDto);

            await productCategoryService.CreateCategory(categoryDto);
            return Json(new { success = true });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await productCategoryService.DeleteCategory(id);
            return NoContent();
        }
    }
}
