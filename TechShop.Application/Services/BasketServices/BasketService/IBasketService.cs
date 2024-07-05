using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.WishlistEntities;

namespace TechShop.Application.Services.BasketServices.BasketService
{
    public interface IBasketService : IBaseService<Basket>
    {
        Task AddToBasket(string userId, int productId);
        Task DeleteFromBasketByProductId(string email, int productId);
        Task DeleteFromBasketById(string userId, int id);
        Task<bool> IsInBasket<TField>(string? email, TField field, Expression<Func<BasketItem, TField>> selector);
        Task<Basket> GetUserBasket(string email);
        Task EncreaseQuantityById(string email, int id);
        Task DecreaseQuantityById(string email, int id);
    }
}
