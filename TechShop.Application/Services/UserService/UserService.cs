using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.UserDto;
using TechShop.Infrastructure.Repositories.BaseRepository;

namespace TechShop.Application.Services.UserService
{
    public class UserService(UserManager<ApplicationUser> userManager) : IUserService
    {
        public IQueryable<ApplicationUser> GetUsers()
        {
            return userManager.Users
                    .Include(x => x.UserProfile)
                    .Include(x => x.Basket)
                        .ThenInclude(x => x.BasketItems)
                            .ThenInclude(x => x.Product)
                                .ThenInclude(p => p.ProductPhotos)
                    .Include(x => x.Basket)
                        .ThenInclude(x => x.BasketItems)
                            .ThenInclude(x => x.Product)
                                .ThenInclude(p => p.Category)
                    .Include(x => x.Wishlist)
                        .ThenInclude(x => x.WishlistItems)
                            .ThenInclude(wi => wi.Product)
                                .ThenInclude(p => p.ProductPhotos)
                    .Include(x => x.Wishlist)
                        .ThenInclude(x => x.WishlistItems)
                            .ThenInclude(wi => wi.Product)
                                .ThenInclude(p => p.Category);
        }
    }
}
