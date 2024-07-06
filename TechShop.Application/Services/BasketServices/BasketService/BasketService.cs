using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechShop.Application.Services.AuthService;
using TechShop.Application.Services.BaseService;
using TechShop.Application.Services.BasketServices.BasketItemService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.BasketServices.BasketService
{
    public class BasketService : BaseService<Basket>, IBasketService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IBasketItemService _basketItemService;

        public BasketService(IBaseRepository<Basket> basketRepository, IBasketItemService basketItemService,
            IUserService userService, IMapper mapper) : base(basketRepository)
        {
            _mapper = mapper;
            _userService = userService;
            _basketItemService = basketItemService;
        }

        public async Task AddToBasket(string email, int productId)
        {
            await BeginTransactionAsync();

            try
            {
                var basket = await GetUserBasket(email);

                if (!await IsInBasket(email, productId, x => x.ProductId))
                    await _basketItemService.AddItemToBasket(basket.Id, productId);
                else
                    throw new Exception("Product is already in the basket");

                await CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task DeleteFromBasketByProductId(string email, int productId)
        {
            var basket = await GetUserBasket(email);

            if (await IsInBasket(basket, productId, x => x.ProductId))
                await _basketItemService.DeleteItemFromBasket(basket, x => x.ProductId == productId);
            else
                throw new Exception("Product is not in the basket");
        }

        public async Task DeleteFromBasketById(string email, int id)
        {
            if (await IsInBasket(email, id, x => x.Id))
                await _basketItemService.DeleteAsync(id);
            else
                throw new Exception("Product is not in the basket");
        }

        public async Task<Basket> GetUserBasket(string email)
        {
            var userId = await _userService.GetUserId(email);
            var basket = await GetBasket()
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if (basket == null)
                basket = await CreateWishlist(userId);

            return basket;
        }

        public async Task<bool> IsInBasket<TField>(string? email, TField field,
            Expression<Func<BasketItem, TField>> selector)
        {
            if (email == null)
                return false;

            var basket = await GetUserBasket(email);

            var productIds = _basketItemService.GetAll()
                .Where(x => x.BasketId == basket.Id)
                .Select(selector);

            return await productIds.ContainsAsync(field);
        }

        public async Task EncreaseQuantityById(string email, int id)
        {
            if (await IsInBasket(email, id, x => x.Id))
                await _basketItemService.EncreaseItemQuantity(id, x => x.Id == id);
            else
                throw new Exception("Product is not in the basket");
        }

        public async Task DecreaseQuantityById(string email, int id)
        {
            if (await IsInBasket(email, id, x => x.Id))
                await _basketItemService.DecreaseItemQuantity(id, x => x.Id == id);
            else
                throw new Exception("Product is not in the basket");
        }

        private async Task<Basket> CreateWishlist(string userId)
        {
            var wishlistDto = new RequestWishlistDto
            {
                UserId = userId,
            };

            var basket = _mapper.Map<Basket>(wishlistDto);
            return await AddAsync(basket);
        }

        private IQueryable<Basket> GetBasket()
        {
            return GetAll()
                .Include(x => x.BasketItems)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductPhotos);
        }

        private async Task<bool> IsInBasket<TField>(Basket basket, TField field,
            Expression<Func<BasketItem, TField>> selector)
        {
            var productIds = _basketItemService.GetAll()
                .Where(x => x.BasketId == basket.Id)
                .Select(selector);

            return await productIds.ContainsAsync(field);
        }
    }
}
