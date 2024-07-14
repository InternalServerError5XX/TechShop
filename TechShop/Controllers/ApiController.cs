using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.AppServices.CacheService;
using TechShop.Application.Services.AppServices.TempDataService;
using TechShop.Application.Services.AuthService;
using TechShop.Application.Services.BasketServices.BasketService;
using TechShop.Application.Services.OrserServices.OrderItemService;
using TechShop.Application.Services.OrserServices.OrserService;
using TechShop.Application.Services.OrserServices.PaymentService;
using TechShop.Application.Services.OrserServices.ShippingInfoServie;
using TechShop.Application.Services.ProductServices.ProductPhotoService;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Application.Services.UserServices.UserProfileService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Application.Services.WishlistServices.WishlistService;
using TechShop.Domain.DTOs.AuthDto;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.FilterDto;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.DTOs.OrderDtos.PaymentDto;
using TechShop.Domain.DTOs.OrderDtos.ShippingInfoDto;
using TechShop.Domain.DTOs.PaginationDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.ProductDtos.ProductPhoto;
using TechShop.Domain.DTOs.UserDtos.RoleDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.DTOs.UserDtos.UserProfileDto;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Domain.Entities.UserEntities;
using TechShopWeb.Filters;

namespace TechShop.Controllers
{
    [Authorize(Roles = "Admin")]
    [TypeFilter(typeof(ApiControllerExceptionFilter))]
    public class ApiController(IAuthService authService, IUserService userService, IProductService productService,
        IProductPhotoService productPhotoService, ITempDataService tempDataService, IWishlistService wishlistService,
        IBasketService basketService, IProductCategoryService productCategoryService, IAdminService adminService,
        IUserProfileService userProfileService, IOrderService orderService, IPaymentService paymentService, 
        IShippingInfoService shippingInfoServie, ICacheService cacheService, IMapper mapper) : Controller
    {
        // ADMIN

        [HttpGet("AdminPanel")]
        [ApiExplorerSettings(GroupName = "Admin")]
        public async Task<IActionResult> AdminPanel()
        {
            var response = await adminService.GetAdminPanel();
            return Ok(response);
        }

        // TEMPDATA

        [HttpPost("SetTempData")]
        [ApiExplorerSettings(GroupName = "Tempdata")]
        public IActionResult SetTempData(string key, string value)
        {
            tempDataService.Set(key, value);
            return Ok();
        }

        [HttpGet("GetTempData")]
        [ApiExplorerSettings(GroupName = "Tempdata")]
        public IActionResult GetTempData(string key)
        {
            var value = tempDataService.Get<string>(key);
            return Ok(value);
        }

        // CACHE

        [HttpPost("SetCache")]
        [ApiExplorerSettings(GroupName = "Cache")]
        public IActionResult SetCache(string key, string value, int absoluteSecs, int slidingSecs)
        {
            var absoluteExpiration = TimeSpan.FromSeconds(absoluteSecs);
            var slidingExpiration = TimeSpan.FromSeconds(slidingSecs);

            cacheService.Set(key, value, absoluteExpiration, slidingExpiration);
            return Ok();
        }

        [HttpGet("GetCache")]
        [ApiExplorerSettings(GroupName = "Cache")]
        public IActionResult GetCache(string key)
        {
            var response = cacheService.Get<string>(key);
            return Ok(response);
        }

        [HttpDelete("RemoveCache")]
        [ApiExplorerSettings(GroupName = "Cache")]
        public IActionResult RemoveCache(string key)
        {
            cacheService.Remove(key);
            return NoContent();
        }

        // SIGNALR

        [HttpGet("CheckUserOnline")]
        [ApiExplorerSettings(GroupName = "SignalR")]
        public IActionResult CheckUserOnline(string id)
        {
            var isOnline = UserHub.IsUserOnline(id);
            return Ok(new { IsOnline = isOnline });
        }

        // AUTH      

        [HttpGet("AuthCheck")]
        [ApiExplorerSettings(GroupName = "Auth")]
        public IActionResult AuthCheck()
        {
            var token = Request.Cookies["token"];

            if (token == null)
                return Unauthorized("Unauthorized");

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ApiExplorerSettings(GroupName = "Auth")]
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

        [AllowAnonymous]
        [HttpPost("Register")]
        [ApiExplorerSettings(GroupName = "Auth")]
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
        [ApiExplorerSettings(GroupName = "Auth")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return Ok();
        }

        // PRODUCTS

        [HttpGet("GetProduct")]
        [ApiExplorerSettings(GroupName = "Products")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await productService.GetProduct(id);
            var response = mapper.Map<ResponseProductDto>(product);

            return Ok(response);
        }

        [HttpGet("GetProducts")]
        [ApiExplorerSettings(GroupName = "Products")]
        public async Task<IActionResult> GetProducts(RequestPaginationDto paginationDto, string? searchTerm, string? orderBy)
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
        [ApiExplorerSettings(GroupName = "Products")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {
            var product = await productService.CreateProduct(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPatch("UpdateProduct")]
        [ApiExplorerSettings(GroupName = "Products")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDto productDto)
        {
            await productService.UpdateProduct(id, productDto);
            var updatedProduct = await productService.GetProduct(id);
            var response = mapper.Map<ResponseProductDto>(updatedProduct);

            return Ok(response);
        }

        [HttpDelete("DeleteProduct")]
        [ApiExplorerSettings(GroupName = "Products")]
        public async Task<IActionResult> Delete(int id)
        {
            await productService.DeleteProduct(id);
            return NoContent();
        }


        [HttpPost("SaveProductPhoto")]
        [ApiExplorerSettings(GroupName = "Products")]
        public async Task<IActionResult> AddProductPhoto([FromForm] IEnumerable<RequestProductPhotoDto> requestProductPhotoDto)
        {
            var photo = await productPhotoService.SavePhotoAsync(requestProductPhotoDto);
            var response = mapper.Map<IEnumerable<ResponseProductPhotoDto>>(photo);

            return CreatedAtAction(nameof(response), new { }, response);
        }

        // CATEGORIES

        [HttpGet("GetCategory")]
        [ApiExplorerSettings(GroupName = "Categories")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await productCategoryService.GetByIdAsync(id);
            var response = mapper.Map<ResponseProductCaregoryDto>(category);

            return Ok(response);
        }

        [HttpGet("GetCategories")]
        [ApiExplorerSettings(GroupName = "Categories")]
        public IActionResult GetCategories()
        {
            var categories = productCategoryService.GetAll();
            var response = mapper.Map<IEnumerable<ResponseProductCaregoryDto>>(categories);

            return Ok(response);
        }

        [HttpPost("CreateCategory")]
        [ApiExplorerSettings(GroupName = "Categories")]
        public async Task<IActionResult> CreateCategory(RequestProductCategoryDto categoryDto)
        {
            var category = await productCategoryService.CreateCategory(categoryDto);
            var response = mapper.Map<ResponseProductCaregoryDto>(category);

            return CreatedAtAction(nameof(response), new { response.Id }, response);
        }

        [HttpPatch("UpdateCategory")]
        [ApiExplorerSettings(GroupName = "Categories")]
        public async Task<IActionResult> UpdateCategory(int id, RequestProductCategoryDto categoryDto)
        {
            await productCategoryService.UpdateCategory(id, categoryDto);
            var category = await productCategoryService.GetByIdAsync(id);
            var response = mapper.Map<ResponseProductCaregoryDto>(category);

            return CreatedAtAction(nameof(response), new { response.Id }, response);
        }

        [HttpDelete("DeleteCategory")]
        [ApiExplorerSettings(GroupName = "Categories")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await productCategoryService.DeleteCategory(id);
            return NoContent();
        }

        // USERS

        [HttpGet("AdminCheck")]
        [ApiExplorerSettings(GroupName = "Users")]
        public IActionResult AdminCheck()
        {
            var check = User.IsInRole("Admin");
            return Ok(check);
        }

        [HttpGet("GetUsers")]
        [ApiExplorerSettings(GroupName = "Users")]
        public IActionResult GetUsers()
        {
            var users = userService.GetUsers();
            var response = mapper.Map<IEnumerable<ApplicationUserDto>>(users);

            return Ok(response);
        }

        [HttpGet("GetUser")]
        [ApiExplorerSettings(GroupName = "Users")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await userService.GetUser(id);
            var response = mapper.Map<ApplicationUserDto>(user);
            response.UserProfile.Role = await userService.GetRoleByUserId(id);

            return Ok(response);
        }

        [HttpGet("GetProfile")]
        [ApiExplorerSettings(GroupName = "Users")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var profile = await userService.GetProfile(userId);
            var response = mapper.Map<ResponseUserProfileDto>(profile);
            response.Role = await userService.GetRoleByUserId(userId);
            response.RoleId = response.Role.Id;

            return Ok(response);
        }

        [HttpGet("GetUserProfile")]
        [ApiExplorerSettings(GroupName = "Users")]
        public async Task<IActionResult> GetUserProfile()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profile = await userService.GetUserProfile(email!);
            var response = mapper.Map<ResponseUserProfileDto>(profile);
            response.Role = await userService.GetRoleByUserId(profile.UserId);
            response.RoleId = response.Role.Id;

            return Ok(response);
        }

        [HttpPost("CreateUser")]
        [ApiExplorerSettings(GroupName = "Users")]
        public async Task<IActionResult> CreateUser(RequestUserDto userDto)
        {
            var user = await userService.CreateUser(userDto);
            var response = mapper.Map<ApplicationUserDto>(user);

            return Ok(response);
        }

        [HttpPatch("UpdateUser")]
        [ApiExplorerSettings(GroupName = "Users")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto userDto)
        {
            await userService.UpdateUser(userDto);
            return Ok();
        }

        [HttpPatch("UpdateUserProfile")]
        [ApiExplorerSettings(GroupName = "Users")]
        public async Task<IActionResult> UpdateUserProfile(int id, RequestUserProfileDto userProfileDto)
        {
            var checkProfile = await userProfileService.GetByIdAsync(id);
            if (checkProfile == null)
                throw new NullReferenceException("Profile not found");

            var profile = mapper.Map<UserProfile>(userProfileDto);
            profile.Id = id;
            profile.CreatedDate = checkProfile.CreatedDate;

            await userService.UpdateProfile(profile);
            var response = mapper.Map<ResponseUserProfileDto>(profile);

            return Ok(response);
        }

        [HttpDelete("DeleteUser")]
        [ApiExplorerSettings(GroupName = "Users")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await userService.DeleteUser(id);
            return NoContent();
        }

        // ROLES

        [HttpPost("CreateRole")]
        [ApiExplorerSettings(GroupName = "Roles")]
        public async Task<IActionResult> CreateRole(RequestRoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await userService.CreateRole(roleDto);
            return Ok(response);
        }

        [HttpPatch("UpdateRole")]
        [ApiExplorerSettings(GroupName = "Roles")]
        public async Task<IActionResult> UpdateRole(IdentityRole identityRole)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await userService.UpdateRole(identityRole);
            return Ok(response);
        }

        [HttpGet("GetRole")]
        [ApiExplorerSettings(GroupName = "Roles")]
        public async Task<IActionResult> GetRole(string id)
        {
            var response = await userService.GetRole(id);
            return Ok(response);
        }

        [HttpGet("GetRoleByUserId")]
        [ApiExplorerSettings(GroupName = "Roles")]
        public async Task<IActionResult> GetRoleByUserId(string userId)
        {
            var response = await userService.GetRoleByUserId(userId);
            return Ok(response);
        }

        [HttpGet("GetRoles")]
        [ApiExplorerSettings(GroupName = "Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var response = await userService.GetRoles();
            return Ok(response);
        }

        [HttpDelete("DeleteRole")]
        [ApiExplorerSettings(GroupName = "Roles")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            await userService.DeleteRole(id);
            return NoContent();
        }

        // WISHLIST

        [HttpPost("AddToWishlist")]
        [ApiExplorerSettings(GroupName = "Wishlist")]
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
        [ApiExplorerSettings(GroupName = "Wishlist")]
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
        [ApiExplorerSettings(GroupName = "Wishlist")]
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
        [ApiExplorerSettings(GroupName = "Wishlist")]
        public async Task<IActionResult> GetWishlist(string email)
        {
            var wishlists = await wishlistService.GetUserWishlist(email);
            var response = mapper.Map<ResponseWishlistDto>(wishlists);

            return Ok(response);
        }

        // BASKET

        [HttpPost("AddToBasket")]
        [ApiExplorerSettings(GroupName = "Basket")]
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
        [ApiExplorerSettings(GroupName = "Basket")]
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
        [ApiExplorerSettings(GroupName = "Basket")]
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
        [ApiExplorerSettings(GroupName = "Basket")]
        public async Task<IActionResult> GetBasket(string email)
        {
            var wishlists = await basketService.GetUserBasket(email!);
            var response = mapper.Map<ResponseBasketDto>(wishlists);

            return Ok(response);
        }

        [HttpPatch("EncreaseBasketItemQuantity")]
        [ApiExplorerSettings(GroupName = "Basket")]
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

        [HttpPatch("DecreaseBasketItemQuantity")]
        [ApiExplorerSettings(GroupName = "Basket")]
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

        // SHIPPING

        [HttpGet("GetShippingInfos")]
        [ApiExplorerSettings(GroupName = "ShippingInfo")]
        public IActionResult GetShippingInfos()
        {
            var shippings = shippingInfoServie.GetAll();
            var response = mapper.Map<IEnumerable<ResponseShippingInfoDto>>(shippings);

            return Ok(response);
        }

        [HttpGet("GetShippingInfo")]
        [ApiExplorerSettings(GroupName = "ShippingInfo")]
        public async Task<IActionResult> GetShippingInfo(int id)
        {
            var shipping = await shippingInfoServie.GetByIdAsync(id);
            if (shipping == null)
                throw new NullReferenceException("Spipping not found");

            var response = mapper.Map<ResponseShippingInfoDto>(shipping);
            return Ok(response);
        }

        [HttpPost("CreateShippingInfo")]
        [ApiExplorerSettings(GroupName = "ShippingInfo")]
        public async Task<IActionResult> CreateShippingInfo(RequestShippingInfoDto shippingInfoDto)
        {
            var shipping = mapper.Map<ShippingInfo>(shippingInfoDto);
            var response = await shippingInfoServie.AddAsync(shipping);

            return CreatedAtAction(nameof(GetShippingInfo), new { id = response.Id }, response);
        }

        [HttpPatch("UpdateShippingInfo")]
        [ApiExplorerSettings(GroupName = "ShippingInfo")]
        public async Task<IActionResult> UpdateShippingInfo(int id, RequestShippingInfoDto shippingInfoDto)
        {
            var shippingCheck = await shippingInfoServie.GetByIdAsync(id);
            if (shippingCheck == null)
                throw new NullReferenceException("Spipping not found");

            var shipping = mapper.Map<ShippingInfo>(shippingInfoDto);
            shipping.Id = shippingCheck.Id;
            shipping.CreatedDate = shippingCheck.CreatedDate;
            await shippingInfoServie.UpdateAsync(shipping);

            var response = mapper.Map<ResponseShippingInfoDto>(shipping);
            return Ok(response);
        }

        [HttpDelete("DeleteShippingInfo")]
        [ApiExplorerSettings(GroupName = "ShippingInfo")]
        public async Task<IActionResult> DeleteShippingInfo(int id)
        {
            await shippingInfoServie.DeleteAsync(id);
            return NoContent();
        }

        // ORDER

        [HttpGet("GetOrders")]
        [ApiExplorerSettings(GroupName = "Order")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = orderService.GetOrders();
            var response = mapper.Map<IEnumerable<ResponseOrderDto>>(orders);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetUserOrders")]
        [ApiExplorerSettings(GroupName = "Order")]
        public async Task<IActionResult> GetUserOrders()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var orders = orderService.GetUsersOrders(email!);
            var response = mapper.Map<IEnumerable<ResponseOrderDto>>(orders);

            return Ok(response);
        }

        [HttpGet("GetOrder")]
        [ApiExplorerSettings(GroupName = "Order")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await orderService.GetOrder(id);
            var response = mapper.Map<ResponseOrderDto>(order);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("CreateOrder")]
        [ApiExplorerSettings(GroupName = "Order")]
        public async Task<IActionResult> CreateOrder(RequestShippingInfoDto shippingInfoDto)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var shippingInfo = mapper.Map<ShippingInfo>(shippingInfoDto);
            var response = await orderService.CreateOrder(email!, shippingInfo);

            return Ok(response);          
        }

        [HttpPatch("UpdateOrder")]
        [ApiExplorerSettings(GroupName = "Order")]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {           
            var shippingInfo = mapper.Map<ShippingInfo>(updateOrderDto.ShippingInfoDto);
            var order = await orderService.UpdateOrder(updateOrderDto.Id, updateOrderDto.OrderStatus, shippingInfo);

            var response = mapper.Map<ResponseOrderDto>(order);
            return Ok(response);
        }

        [HttpPatch("CancelOrder")]
        [ApiExplorerSettings(GroupName = "Order")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await orderService.CancelOrder(id);
            var response = mapper.Map<ResponseOrderDto>(order);

            return Ok(response);
        }

        [HttpDelete("DeleteOrder")]
        [ApiExplorerSettings(GroupName = "Order")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await orderService.DeleteOrder(id);
            return NoContent();
        }

        // PAYMENT

        [HttpGet("GetPayments")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public IActionResult GetPayments()
        {
            var payments = paymentService.GetAll();
            var response = mapper.Map<IEnumerable<ResponsePaymentDto>>(payments);

            return Ok(response);
        }

        [HttpGet("GetPayment")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payments = await paymentService.GetByIdAsync(id);
            var response = mapper.Map<ResponsePaymentDto>(payments);

            return Ok(response);
        }

        [HttpGet("GetStripeSession")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public async Task<IActionResult> GetStripeSession(string sessionId)
        {
            var response = await paymentService.GetStripeSession(sessionId);
            return Ok(response);
        }

        [HttpPost("CreatePayment")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public async Task<IActionResult> CreatePayment(RequestPaymentDto paymentDto)
        {
            var payment = mapper.Map<Payment>(paymentDto);
            var response = await paymentService.AddAsync(payment);

            return CreatedAtAction(nameof(GetPayment), new { id = response.Id }, response);
        }

        [Authorize]
        [HttpPost("CreatePaymentIntent")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public async Task<IActionResult> CreatePaymentIntent()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var basket = await basketService.GetUserBasket(email!);
            var response = await paymentService.CreatePaymentIntent(basket);
            return Ok(response);
        }

        [HttpGet("success")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public IActionResult Success()
        {
            return Ok(new { message = "Payment successful!" });
        }

        [HttpGet("cancel")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public IActionResult Cancel()
        {
            return Ok(new { message = "Payment canceled." });
        }

        [HttpPatch("UpdatePayment")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public async Task<IActionResult> UpdatePayment(int id)
        {
            var payment = await paymentService.UpdatePayment(id);
            var response = mapper.Map<ResponsePaymentDto>(payment);

            return Ok(response);
        }        

        [HttpDelete("DeletePayment")]
        [ApiExplorerSettings(GroupName = "Payment")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await paymentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
