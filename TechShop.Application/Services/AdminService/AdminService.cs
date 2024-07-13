using AutoMapper;
using TechShop.Application.Services.AppServices.CacheService;
using TechShop.Application.Services.OrserServices.OrserService;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.AdminDto;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;

namespace TechShop.Application.Services.AdminService
{
    public class AdminService(IUserService userService, IProductService productService, ICacheService cacheService,
        IProductCategoryService productCategoryService, IOrderService orderService, IMapper mapper) : IAdminService
    {
        private readonly string cacheKey = "AdminPanelCacheKey";

        public async Task<ResponseAdminDto> GetAdminPanel()
        {
            var users = userService.GetUsers();
            var roles = await userService.GetRoles();
            var categories = productCategoryService.GetAll();
            var products = productService.GetProducts();
            var orders = orderService.GetOrders();
            await orderService.UpdateOrdersPaymentStatusTransaction(orders);

            var usersResponse = mapper.Map<IEnumerable<ApplicationUserDto>>(users);

            foreach (var user in usersResponse)
                user.UserProfile.IsOnline = UserHub.IsUserOnline(user.Id);

            var categoriesResponse = mapper.Map<IEnumerable<ResponseProductCaregoryDto>>(categories);
            var productsReponse = mapper.Map<IEnumerable<ResponseProductDto>>(products);
            var ordersResponse = mapper.Map<IEnumerable<ResponseOrderDto>>(orders);

            return new ResponseAdminDto
            {
                Users = usersResponse,
                Roles = roles,
                Categories = categoriesResponse,
                Products = productsReponse,
                Orders = ordersResponse
            };
        }

        public async Task<ResponseAdminDto> GetCachedAdminPanel()
        {
            var response = cacheService.Get<ResponseAdminDto>(cacheKey);
            if (response == null)
            {
                response = await GetAdminPanel();
                cacheService.Set(cacheKey, response, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(2));
            }

            return response;
        }

        public void RemoveCachedAdminPanel()
        {
            cacheService.Remove(cacheKey);
        }
    }
}
