using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.WishlistEntities;

namespace TechShop.Application.Services.WishlistServices.WishlistItemService
{
    public interface IWishlistItemService : IBaseService<WishlistItem>
    {
        Task AddItemToWishlist(int wishlistId, int productId);
        Task DeleteItemFromWishlist(Wishlist wishlist, Expression<Func<WishlistItem, bool>> expression);
    }
}
