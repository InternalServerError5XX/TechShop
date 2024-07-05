using System.Linq.Expressions;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.WishlistEntities;

namespace TechShop.Application.Services.BasketServices.BasketItemService
{
    public interface IBasketItemService : IBaseService<BasketItem>
    {
        Task AddItemToBasket(int basketId, int productId);
        Task DeleteItemFromBasket(Basket basket, Expression<Func<BasketItem, bool>> expression);
        Task EncreaseItemQuantity(int id, Expression<Func<BasketItem, bool>> expression);
        Task DecreaseItemQuantity(int id, Expression<Func<BasketItem, bool>> expression);
    }
}
