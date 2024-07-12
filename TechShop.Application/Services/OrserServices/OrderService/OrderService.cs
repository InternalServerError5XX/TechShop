using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TechShop.Application.Services.BaseService;
using TechShop.Application.Services.BasketServices.BasketItemService;
using TechShop.Application.Services.BasketServices.BasketService;
using TechShop.Application.Services.OrserServices.OrderItemService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.OrserServices.OrserService
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IBasketItemService _basketItemService;
        private readonly IOrderItemService _orderItemService;    
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public OrderService(IBaseRepository<Order> orderRepository, IOrderItemService orderItemService, 
            IBasketService basketService, IBasketItemService basketItemService, IMapper mapper) : base(orderRepository)
        {
            _basketItemService = basketItemService;
            _orderItemService = orderItemService;
            _basketService = basketService;
            _mapper = mapper;
        }

        public IQueryable<Order> GetOrders()
        {
            return GetAll()
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductPhotos)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Category)
                .Include(x => x.ShippingInfo)
                .Include(x => x.Payment);
        }

        public IQueryable<Order> GetUsersOrders(string email)
        {
            return GetAll()
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductPhotos)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Category)
                .Include(x => x.ShippingInfo)
                .Include(x => x.Payment)
                .Where(x => x.User.Email == email);
        }

        public async Task<Order?> GetOrder(int id)
        {
            return await GetOrders()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order> CreateOrder(string email, Order order)
        {
            await BeginTransactionAsync();

            try
            {
                var basket = await _basketService.GetUserBasket(email);
                var items = _mapper.Map<IEnumerable<OrderItem>>(basket.BasketItems).ToList();

                var orderResponse = await AddAsync(order);

                foreach (var item in items)
                    item.OrderId = orderResponse.Id;

                await _orderItemService.AddRangeAsync(items);
                await _basketItemService.DeleteRangeAsync(basket.BasketItems);

                await CommitTransactionAsync();
                return orderResponse;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }
    }
}
