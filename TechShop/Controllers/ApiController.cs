using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechShop.Application.Services.AuthService;
using TechShop.Domain.DTOs.AuthDto;
using AutoMapper;
using TechShop.Domain.DTOs.PaginationDto;
using TechShop.Domain.DTOs.FilterDto;
using TechShop.Application.Services.TempDataService;
using TechShopWeb.Filters;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Application.Services.ProductServices.ProductPhotoService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;
using TechShop.Application.Services.WishlistServices.WishlistService;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TechShop.Application.Services.BasketServices.BasketService;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.ProductDtos.ProductPhoto;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Application.Services.UserServices.UserProfileService;
using TechShop.Domain.DTOs.UserDtos.UserProfileDto;

namespace TechShop.Controllers
{
    [TypeFilter(typeof(ApiControllerExceptionFilter))]
    public class ApiController(IAuthService authService, IUserService userService, IProductService productService,
        IProductPhotoService productPhotoService, ITempDataService tempDataService, IWishlistService wishlistService, 
        IBasketService basketService, IProductCategoryService productCategoryService, IMapper mapper) : Controller
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
            var token = Request.Cookies["token"];

            if (token == null)
                return Unauthorized("Unauthorized");

            return Ok(token);
        }

        [Authorize]
        [HttpGet("AdminCheck")]
        public IActionResult AdminCheck()
        {
            var check = User.IsInRole("Admin");
            return Ok(check);
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var response = await userService.GetRoles();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
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
            Response.Cookies.Delete("token");
            return Ok();
        }

        [HttpGet("GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await productService.GetProduct(id);
            var response = mapper.Map<ResponseProductDto>(product);

            return Ok(response);
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts(RequestPaginationDto paginationDto, string? searchTerm, 
            string? orderBy, bool? isAsc)
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

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] RequestProductDto productDto)
        {        
            var product = await productService.CreateProduct(productDto);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var category = productCategoryService.GetAll();
            var response = mapper.Map<IEnumerable<ResponseProductCaregoryDto>>(category);

            return CreatedAtAction(nameof(response), new { }, response);
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(RequestProductCategoryDto categoryDto)
        {
            var category = await productCategoryService.CreateCategory(categoryDto);
            var response = mapper.Map<ResponseProductCaregoryDto>(category);

            return CreatedAtAction(nameof(response), new { response.Id }, response);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = userService.GetUsers();
            var response = mapper.Map<IEnumerable<ApplicationUserDto>>(users);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profile = await userService.GetUserProfile(email!);
            var response = mapper.Map<ResponseUserProfileDto>(profile);

            return Ok(response);
        }

        [HttpPost("SavePhoto")]
        public async Task<IActionResult> AddProductPhoto([FromForm] IEnumerable<RequestProductPhotoDto> requestProductPhotoDto)
        {
            var photo = await productPhotoService.SavePhoto(requestProductPhotoDto);
            var response = mapper.Map<IEnumerable<ResponseProductPhotoDto>>(photo);

            return CreatedAtAction(nameof(response), new { }, response);
        }

        [HttpDelete("DeleteProductPhoto")]
        public async Task<IActionResult> DeleteProductPhoto(int id)
        {
            await productPhotoService.DeletePhoto(id);
            return NoContent();
        }

        [HttpPost("AddToWishlist")]
        public async Task<IActionResult> AddToWishlist([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                await wishlistService.AddToWishlist(email, productId);
                return Ok();
            }

            return Unauthorized("Unauthorized");
        }

        [HttpDelete("DeleteFromWishlistByProductId")]
        public async Task<IActionResult> DeleteFromWishlistByProductId([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                await wishlistService.DeleteFromWishlistByProductId(email, productId);
                return NoContent();
            }

            return Unauthorized("Unauthorized");
        }

        [HttpGet("IsInWishlist")]
        public async Task<IActionResult> IsInWishlist(int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                var response = await wishlistService.IsInWishlist(email, id, x => x.ProductId);
                return Ok(response);
            }

            return Unauthorized("Unauthorized");
        }

        [HttpGet("GetWishlist")]
        public async Task<IActionResult> GetWishlist(string email)
        {
            var wishlists = await wishlistService.GetUserWishlist(email);
            var response = mapper.Map<ResponseWishlistDto>(wishlists);

            return Ok(response);
        }

        [HttpPost("AddToBasket")]
        public async Task<IActionResult> AddToBasket([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                await basketService.AddToBasket(email!, productId);
                return Ok();
            }

            return Unauthorized("Unauthorized");
        }

        [HttpDelete("DeleteFromBasketByProductId")]
        public async Task<IActionResult> DeleteFromBasketByProductId([Required] int productId)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                await basketService.DeleteFromBasketByProductId(email!, productId);
                return Ok();
            }

            return Unauthorized("Unauthorized");
        }

        [HttpGet("IsInBasket")]
        public async Task<IActionResult> IsInBasket(int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                var response = await basketService.IsInBasket(email, id, x => x.ProductId);
                return Ok(response);
            }

            return Unauthorized("Unauthorized");
        }

        [HttpGet("GetBasket")]
        public async Task<IActionResult> GetBasket(string email)
        {
            var wishlists = await basketService.GetUserBasket(email!);
            var response = mapper.Map<ResponseBasketDto>(wishlists);

            return Ok(response);
        }

        [HttpPost("EncreaseBasketItemQuantity")]
        public async Task<IActionResult> EncreaseBasketItemQuantity([Required] int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                await basketService.EncreaseQuantityById(email!, id);
                return Ok();
            }

            return Unauthorized("Unauthorized");
        }

        [HttpPost("DecreaseBasketItemQuantity")]
        public async Task<IActionResult> DecreaseBasketItemQuantity([Required] int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                await basketService.DecreaseQuantityById(email!, id);
                return Ok();
            }

            return Unauthorized("Unauthorized");
        }
    }
}
