using TechShop.Application.Services.BaseService;
using TechShop.Domain.DTOs.UserDto;

namespace TechShop.Application.Services.UserService
{
    public interface IUserService
    {
        IQueryable<ApplicationUser> GetUsers();
    }
}
