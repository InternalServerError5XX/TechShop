using AutoMapper;
using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.BasketDtos.BasketItemDto;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;
using TechShop.Infrastructure.Repositories.ProductRepositories.ProductRepositoty;

namespace TechShop.Application.Services.BasketServices.BasketItemService
{
    public class BasketItemService : BaseService<BasketItem>, IBasketItemService
    {
        private readonly IProductRepositoty _productRepositoty;
        private readonly IMapper _mapper;

        public BasketItemService(IBaseRepository<BasketItem> basketItemsRepository, IMapper mapper,
            IProductRepositoty productRepositoty) : base(basketItemsRepository)
        {
            _mapper = mapper;
            _productRepositoty = productRepositoty;
        }

        public async Task AddItemToBasket(int basketId, int productId)
        {
            var item = await _productRepositoty.GetByIdAsync(productId);
            if (item == null)
                throw new NullReferenceException("Product not found");

            var basketItemDto = new RequestBasketItemDto
            {
                BasketId = basketId,
                ProductId = productId,
                Quantity = 1
            };

            var basketItem = _mapper.Map<BasketItem>(basketItemDto);
            await AddAsync(basketItem);
        }

        public async Task DeleteItemFromBasket(Basket basket, Expression<Func<BasketItem, bool>> expression)
        {
            var response = basket.BasketItems
                .AsQueryable()
                .FirstOrDefault(expression);

            if (response == null)
                throw new NullReferenceException("Product is not in the basket");

            await DeleteAsync(response.Id);
        }

        public async Task EncreaseItemQuantity(int id, Expression<Func<BasketItem, bool>> expression)
        {
            var response = await GetByIdAsync(id);
            response.Quantity += 1;

            if (response.Quantity > 99)
                throw new Exception("Product quantity must be less than 99");

            await UpdateAsync(response);
        }

        public async Task DecreaseItemQuantity(int id, Expression<Func<BasketItem, bool>> expression)
        {
            var response = await GetByIdAsync(id);
            response.Quantity -= 1;

            if (response.Quantity < 1)
                throw new Exception("Product quantity must be over than 1");

            await UpdateAsync(response);
        }
    }
}
