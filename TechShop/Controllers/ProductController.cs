using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return View(productDto);
            var response = await productService.CreateProduct(productDto);

            return RedirectToAction(nameof(GetById), new { id = response.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await productService.GetByIdAsync(id);
            var response = mapper.Map<CreateProductDto>(product);
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CreateProductDto productDto)
        {
            if (!ModelState.IsValid)
                return View(productDto);
            await productService.UpdateProduct(id, productDto);

            return RedirectToAction(nameof(GetById), new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteProduct(id);

            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer) && referer.Contains("Adminpanel") && referer.Contains("GetAll"))
            {
                return Redirect(referer);
            }
            else
            {
                return RedirectToAction("Adminpanel", "Admin");
            }
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return PartialView("~/Views/Product/_CreateCategoryModal.cshtml", new RequestProductCategoryDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(RequestProductCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return PartialView("~/Views/Product/_CreateCategoryModal.cshtml", categoryDto);

            await productCategoryService.CreateCategory(categoryDto);

            return Json(new { success = true });
        }

    }
}
