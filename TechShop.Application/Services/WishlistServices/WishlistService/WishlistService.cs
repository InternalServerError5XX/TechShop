using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TechShop.Application.Services.AuthService;
using TechShop.Application.Services.BaseService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Application.Services.WishlistServices.WishlistItemService;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;
using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.WishlistServices.WishlistService
{
    public class WishlistService : BaseService<Wishlist>, IWishlistService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IWishlistItemService _wishlistItemService;

        public WishlistService(IBaseRepository<Wishlist> wishlistRepository, IWishlistItemService wishlistItemService,
            IUserService userService, IMapper mapper) : base(wishlistRepository)
        {
            _mapper = mapper;
            _userService = userService;
            _wishlistItemService = wishlistItemService;
        }

        public async Task AddToWishlist(string email, int productId)
        {
            await BeginTransactionAsync();

            try
            {
                var wishlist = await GetUserWishlist(email);

                if (!await IsInWishlist(wishlist, productId, x => x.ProductId))
                    await _wishlistItemService.AddItemToWishlist(wishlist.Id, productId);
                else
                    throw new Exception("Product is already in the wishlist");

                await CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync(ex);
                throw;
            }
        }

        public async Task DeleteFromWishlistByProductId(string email, int productId)
        {
            var wishlist = await GetUserWishlist(email);

            if (await IsInWishlist(wishlist, productId, x => x.ProductId))
                await _wishlistItemService.DeleteItemFromWishlist(wishlist, x => x.ProductId == productId);
            else
                throw new Exception("Product is not in the wishlist");
        }

        public async Task DeleteFromWishlistById(string email, int id)
        {
            var wishlist = await GetUserWishlist(email);

            if (await IsInWishlist(wishlist, id, x => x.Id))
                await _wishlistItemService.DeleteItemFromWishlist(wishlist, x => x.Id == id);
            else
                throw new Exception("Product is not in the wishlist");
        }

        public async Task<Wishlist> GetUserWishlist(string email)
        {
            var userId = await _userService.GetUserId(email);
            var wishlist = await GetWishlists()
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if (wishlist == null)
                wishlist = await CreateWishlist(userId);

            return wishlist;
        }

        public async Task<bool> IsInWishlist<TField>(string? email, TField field,
            Expression<Func<WishlistItem, TField>> selector)
        {
            if (email == null)
                return false;

            var wishlist = await GetUserWishlist(email);

            var productIds = _wishlistItemService.GetAll()
                .Where(x => x.WishlistId == wishlist.Id)
                .Select(selector);

            return await productIds.ContainsAsync(field);
        }        

        private async Task<Wishlist> CreateWishlist(string userId)
        {
            var wishlistDto = new RequestWishlistDto
            {
                UserId = userId,
            };

            var wishlist = _mapper.Map<Wishlist>(wishlistDto);
            return await AddAsync(wishlist);
        }

        private IQueryable<Wishlist> GetWishlists()
        {
            return GetAll()
                .Include(x => x.WishlistItems)
                    .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductPhotos);
        }

        private async Task<bool> IsInWishlist<TField>(Wishlist wishlist, TField field,
            Expression<Func<WishlistItem, TField>> selector)
        {
            var productIds = _wishlistItemService.GetAll()
                .Where(x => x.WishlistId == wishlist.Id)
                .Select(selector);

            return await productIds.ContainsAsync(field);
        }
    }
}
