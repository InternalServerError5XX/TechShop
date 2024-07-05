using AutoMapper;
using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.WishlistDtos.WishlistItemDto;
using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;
using TechShop.Infrastructure.Repositories.ProductRepositories.ProductRepositoty;
using TechShop.Infrastructure.Repositories.WishlistRepositories.WishlistItemRepository;

namespace TechShop.Application.Services.WishlistServices.WishlistItemService
{
    public class WishlistItemService : BaseService<WishlistItem>, IWishlistItemService
    {
        private readonly IProductRepositoty _productRepositoty;
        private readonly IMapper _mapper;

        public WishlistItemService(IBaseRepository<WishlistItem> wishlistItemRepository, IMapper mapper,
            IProductRepositoty productRepositoty) : base(wishlistItemRepository)
        {
            _mapper = mapper;
            _productRepositoty = productRepositoty;
        }

        public async Task AddItemToWishlist(int wishlistId, int productId)
        {
            var item = await _productRepositoty.GetByIdAsync(productId);
            if (item == null)
                throw new NullReferenceException("Product not found");

            var wishlistItemDto = new RequestWishlistItemDto
            {
                WishlistId = wishlistId,
                ProductId = productId              
            };

            var wishlistItem = _mapper.Map<WishlistItem>(wishlistItemDto);
            await AddAsync(wishlistItem);
        }

        public async Task DeleteItemFromWishlist(Wishlist wishlist, int productId)
        {
            var response = wishlist.WishlistItems.FirstOrDefault(x => x.ProductId == productId);
            if (response == null)
                throw new NullReferenceException("Product is not in the wishlist");

            await DeleteAsync(response.Id);
        }

        public async Task DeleteItemFromWishlist(Wishlist wishlist, Expression<Func<WishlistItem, bool>> expression)
        {
            var response = wishlist.WishlistItems
                .AsQueryable()
                .FirstOrDefault(expression);

            if (response == null)
                throw new NullReferenceException("Product is not in the wishlist");

            await DeleteAsync(response.Id);
        }
    }
}
