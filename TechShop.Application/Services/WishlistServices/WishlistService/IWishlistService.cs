using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.WishlistEntities;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.WishlistServices.WishlistService
{
    public interface IWishlistService : IBaseService<Wishlist>
    {
        Task AddToWishlist(string userId, int productId);
        Task DeleteFromWishlistByProductId(string email, int productId);
        Task DeleteFromWishlistById(string email, int id);
        Task<bool> IsInWishlist<TField>(string? email, TField field, Expression<Func<WishlistItem, TField>> selector);
        Task<Wishlist> GetUserWishlist(string email);
    }
}
