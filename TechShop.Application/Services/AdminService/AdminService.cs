using AutoMapper;
using TechShop.Application.Services.AppServices.CacheService;
using TechShop.Application.Services.OrserServices.OrserService;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.AdminDto;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.DTOs.OrderDtos.StatsDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.Entities.OrderEntities;

namespace TechShop.Application.Services.AdminService
{
    public class AdminService(IUserService userService, IProductService productService, ICacheService cacheService,
        IProductCategoryService productCategoryService, IOrderService orderService, IMapper mapper) : IAdminService
    {
        private readonly string adminCacheKey = "AdminPanelCacheKey";

        public async Task<ResponseAdminDto> GetAdminPanel()
        {
            var users = userService.GetUsers();
            var roles = await userService.GetRoles();
            var categories = productCategoryService.GetAll();
            var products = productService.GetProducts();
            var orders = orderService.GetOrders();
            var stats = await GetOrdersStats(orders, null, null);

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
                Orders = ordersResponse,
                OrdersStats = stats
            };
        }

        public async Task<ResponseAdminDto> GetCachedAdminPanel()
        {
            var response = cacheService.Get<ResponseAdminDto>(adminCacheKey);
            if (response == null)
            {
                response = await GetAdminPanel();
                cacheService.Set(adminCacheKey, response, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(2));
            }

            return response;
        }

        public async Task<OrdersStatsDto> GetOrdersStats(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Today.AddMonths(-3).Date;

            if (!endDate.HasValue)
                endDate = DateTime.Today.Date;

            var orders = orderService.GetOrders();
            var profits = orderService.CalculateProfit(orders, startDate, endDate);
            var stats = await orderService.GetOrderStatusStatistics(orders, startDate, endDate);

            return new OrdersStatsDto
            {
                OrdersProfit = profits,
                OrderStatus = stats
            };
        }

        public async Task<OrdersStatsDto> GetOrdersStats(IQueryable<Order> orders, DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
                startDate = DateTime.Today.AddMonths(-3).Date;

            if (!endDate.HasValue)
                endDate = DateTime.Today.Date;

            var profits = orderService.CalculateProfit(orders, startDate, endDate);
            var stats = await orderService.GetOrderStatusStatistics(orders, startDate, endDate);

            return new OrdersStatsDto
            {
                OrdersProfit = profits,
                OrderStatus = stats
            };
        }

        public void RemoveCachedAdminPanel()
        {
            cacheService.Remove(adminCacheKey);
        }
    }
}
