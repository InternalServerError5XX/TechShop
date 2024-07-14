using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using TechShop.Application.Services.AdminService;
using TechShop.Application.Services.BaseService;
using TechShop.Application.Services.BasketServices.BasketItemService;
using TechShop.Application.Services.BasketServices.BasketService;
using TechShop.Application.Services.OrserServices.PaymentService;
using TechShop.Application.Services.OrserServices.ShippingInfoServie;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.Entities.OrderEntities;
using TechShop.Domain.Enums;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.OrserServices.OrserService
{
    public class OrderService : BaseService<Order>, IOrderService
    {      
        private readonly IShippingInfoService _shippingInfoService;
        private readonly IBasketItemService _basketItemService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public OrderService(IBaseRepository<Order> orderRepository, IPaymentService paymentService, IUserService userService,
            IBasketService basketService, IBasketItemService basketItemService, IShippingInfoService shippingInfoService,
            IServiceProvider serviceProvider, IMapper mapper) : base(orderRepository)
        {           
            _shippingInfoService = shippingInfoService;
            _basketItemService = basketItemService;
            _serviceProvider = serviceProvider;
            _paymentService = paymentService;
            _basketService = basketService;          
            _userService = userService;
            _mapper = mapper;
        }

        private void RemoveAdminChache()
        {
            var adminChacheService = _serviceProvider.GetService<IAdminService>();
            if (adminChacheService == null)
                throw new Exception("Couldn't start Admin Service");

            adminChacheService.RemoveCachedAdminPanel();
        }

        public IQueryable<Order> GetOrders()
        {
            return GetAll()
                .AsSplitQuery()
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
                .AsSplitQuery()
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

        public async Task<Order> GetOrder(int id)
        {
            var order = await GetOrders()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
                throw new NullReferenceException("Order not found");

            return order;
        }

        public async Task<string> CreateOrder(string email, ShippingInfo shippingInfo)
        {
            await BeginTransactionAsync();

            try
            {
                var basket = await _basketService.GetUserBasket(email);
                if (!basket.BasketItems.Any())
                    throw new NullReferenceException("Order list is empty");

                var userId = await _userService.GetUserId(email);

                var shippingInfoResponse = await _shippingInfoService.AddAsync(shippingInfo);

                var order = new Order
                {
                    UserId = userId,
                    ShippingInfo = shippingInfoResponse,
                    ShippingInfoId = shippingInfo.Id,
                    OrderItems = _mapper.Map<IEnumerable<OrderItem>>(basket.BasketItems).ToList()
                };               

                foreach (var item in order.OrderItems)
                    item.OrderId = order.Id;

                var paymentResponse = await _paymentService.CreatePaymentIntent(basket);
                var payment = new Payment
                {
                    SessionId = paymentResponse.Id,                    
                    Amount = (decimal)paymentResponse.AmountTotal! / 100
                };

                if (paymentResponse.Status == "open")
                    payment.Status = PaymentStatus.Pending;
                else
                    payment.Status = PaymentStatus.Failed;

                order.Payment = payment;
                order.PaymentId = order.Payment.Id;
                await _paymentService.AddAsync(payment);

                order.Status = OrderStatus.Pending;

                var orderResponse = await AddAsync(order);
                await _basketItemService.DeleteRangeAsync(basket.BasketItems);

                await CommitTransactionAsync();
                RemoveAdminChache();
                return paymentResponse.Url;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task UpdateOrdersPaymentStatusTransaction(IQueryable<Order> orders)
        {
            if (!await orders.AnyAsync(x => x.Status == OrderStatus.Pending))
                return;

            await BeginTransactionAsync();

            try
            {
                var orderlist = await orders
                    .AsNoTracking()
                    .Where(x => x.Status == OrderStatus.Pending)
                    .ToListAsync();

                await _paymentService.UpdatePayment(orderlist);

                foreach (var order in orderlist)
                {
                    if (order.Payment.Status == PaymentStatus.Failed)
                    {
                        order.Status = OrderStatus.Cancelled;
                        RemoveAdminChache();
                    }
                }

                await UpdateRangeAsync(orderlist);
                await CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task<Order> UpdateOrdersPaymentStatusTransaction(Order order)
        {
            if (order.Status != OrderStatus.Pending)
                return order;

            await BeginTransactionAsync();

            try
            {
                var response = await _paymentService.UpdatePayment(order);
                if (response.Status == PaymentStatus.Failed)
                {
                    order.Status = OrderStatus.Cancelled;
                    RemoveAdminChache();
                }

                await CommitTransactionAsync();
                return order;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task<Order> UpdateOrder(int id, OrderStatus orderStatus, ShippingInfo shippingInfo)
        {
            await BeginTransactionAsync();

            try
            {
                var order = await GetOrder(id);
                if (order == null)
                    throw new NullReferenceException("Spipping not found");

                shippingInfo.Id = order.ShippingInfo.Id;
                shippingInfo.CreatedDate = order.ShippingInfo.CreatedDate;
                shippingInfo.UpdatedDate = DateTime.Now;

                order.Status = orderStatus;
                order.ShippingInfo = shippingInfo;

                var response = await _paymentService.UpdatePayment(order);
                if (response.Status == PaymentStatus.Failed)
                    order.Status = OrderStatus.Cancelled;

                await UpdateAsync(order);
                await CommitTransactionAsync();
                RemoveAdminChache();
                return order;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task<Order> CancelOrder(int id)
        {
            var status = await GetOrders()
                .Where(x => x.Id == id)
                .Select(x => x.Status)
                .FirstOrDefaultAsync();

            if (status != OrderStatus.Pending)
                throw new InvalidOperationException("Unable to delete a started order");

            await BeginTransactionAsync();

            try
            {
                var order = await GetOrder(id);
                if (order == null)
                    throw new NullReferenceException("Order not found");

                await UpdateOrdersPaymentStatus(order);
                if (order.Status != OrderStatus.Pending)
                    throw new InvalidOperationException("Unable to delete a started order");

                if (order.Payment.Status == PaymentStatus.Completed)
                {
                    await _paymentService.RefundPayment(order);
                    order.Payment.Status = PaymentStatus.Refund;
                    order.Status = OrderStatus.Cancelled;
                }                

                await UpdateAsync(order);
                RemoveAdminChache();
                await CommitTransactionAsync();
                return order;
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task DeleteOrder(int id)
        {
            var status = await GetOrders()
                .Where(x => x.Id == id)
                .Select(x => x.Payment.Status)
                .FirstOrDefaultAsync();

            if (status == PaymentStatus.Completed)
                throw new InvalidOperationException("Unable to delete a paid order");

            await BeginTransactionAsync();

            try
            {
                var order = await GetOrder(id);
                if (order == null)
                    throw new NullReferenceException("Order not found");

                await UpdateOrdersPaymentStatus(order);
                if (status == PaymentStatus.Completed)
                    throw new InvalidOperationException("Unable to delete a paid order");

                await DeleteAsync(id);
                await _paymentService.DeleteAsync(order.PaymentId);
                await _shippingInfoService.DeleteAsync(order.ShippingInfoId);               
                await CommitTransactionAsync();
                RemoveAdminChache();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        private async Task<Order> UpdateOrdersPaymentStatus(Order order)
        {
            if (order.Status != OrderStatus.Pending)
                return order;

            var response = await _paymentService.UpdatePayment(order);
            if (response.Status == PaymentStatus.Failed)
            {
                order.Status = OrderStatus.Cancelled;
                RemoveAdminChache();
            }

            return order;
        }
    }
}
