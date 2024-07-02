using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AuthService;
using TechShop.Domain.DTOs.AuthDto;
using TechShop.Domain.Entities;
using TechShop.Domain.DTOs.UserDto;
using AutoMapper;
using TechShop.Domain.DTOs.ProductDto;
using TechShop.Application.Services.ProductService;
using TechShop.Application.Services.UserService;
using TechShop.Application.Services.ProductPhotoService;
using TechShop.Domain.DTOs.ProductPhoto;
using TechShop.Domain.DTOs.PaginationDto;
using TechShop.Domain.DTOs.FilterDto;
using Newtonsoft.Json.Linq;
using TechShop.Application.Services.TempDataService;

namespace TechShop.Controllers
{
    public class ApiController(IAuthService authService, IUserService userService, IProductService productService,
        IProductPhotoService productPhotoService, ITempDataService tempDataService, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Swagger()
        {
            return await Task.FromResult(View());
        }

        [HttpPost("SetTempData")]
        public IActionResult SetTempData()
        {
            tempDataService.Set("TestKey", "Test Value");
            return NoContent();
        }

        [HttpGet("GetTempData")]
        public IActionResult GetTempData()
        {
            var value = tempDataService.Get<string>("TestKey");
            return Ok(value);
        }

        [Authorize]
        [HttpGet("AuthCheck")]
        public IActionResult AuthCheck()
        {
            try
            {
                var token = Request.Cookies["token"];

                if (token == null)
                    return Unauthorized("Unauthorized");

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("AdminCheck")]
        public IActionResult AdminCheck()
        {
            try
            {
                var check = User.IsInRole("Admin");
                return Ok(check);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid) 
                    return BadRequest("Wrong input");

                var token = await authService.Login(loginDto);

                Response.Cookies.Append(
                    "token",
                    token.TokenValue,
                    token.CookieOptions);

                return Ok(new
                {
                    Token = token.TokenValue,
                    Expires = token.Expiration
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) 
                    return BadRequest("Wrong input");
                
                var token = await authService.Register(registerDto);

                return Ok(new
                {
                    Token = token.TokenValue,
                    Expires = token.Expiration,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("token");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await productService.GetProduct(id);
                var response = mapper.Map<ResponseProductDto>(product);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts(RequestPaginationDto paginationDto, string? searchTerm, 
            string? orderBy, bool? isAsc)
        {
            try
            {
                var filterDto = new RequestFilterDto<Product>
                {
                    SearchTerm = string.IsNullOrWhiteSpace(searchTerm) ? null :
                        x => x.Brand.ToLower().Contains(searchTerm.ToLower()) ||
                        x.Model.ToLower().Contains(searchTerm.ToLower()) ||
                        x.Category.Name.ToLower().Contains(searchTerm.ToLower()),

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
                }

                var query = productService.GetFilteredQuery(productService.GetProducts(), filterDto);
                var products = await productService.GetPaginated(query, paginationDto);
                var response = mapper.Map<ResponsePaginationDto<ResponseProductDto>>(products);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] RequestProductDto productDto)
        {
            try
            {
                if (!productDto.ProductPhotos.Any())
                    return BadRequest("Product photos are required.");

                var product = await productService.CreateProduct(productDto);

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = userService.GetUsers();
                var response = mapper.Map<IEnumerable<ApplicationUserDto>>(users);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SavePhoto")]
        public async Task<IActionResult> AddProductPhoto([FromForm] IEnumerable<RequestProductPhotoDto> requestProductPhotoDto)
        {
            try
            {
                var photo = await productPhotoService.SavePhoto(requestProductPhotoDto);
                var response = mapper.Map<IEnumerable<ResponseProductPhotoDto>>(photo);

                return CreatedAtAction(nameof(response), new { }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteProductPhoto")]
        public async Task<IActionResult> DeleteProductPhoto(int id)
        {
            try
            {
                await productPhotoService.DeletePhoto(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
