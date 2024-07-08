using AutoMapper;
using TechShop.Application.Services.ProductServices.ProductService;
using TechShop.Application.Services.UserServices.UserService;
using TechShop.Domain.DTOs.AdminDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCategoryService;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;

namespace TechShop.Application.Services.AdminService
{
    public class AdminService(IUserService userService, IProductService productService, 
        IProductCategoryService productCategoryService, IMapper mapper) : IAdminService
    {
        public async Task<ResponseAdminDto> GetAdminPanel()
        {
            var users = userService.GetUsers();
            var roles = await userService.GetRoles();
            var categories = productCategoryService.GetAll();
            var products = productService.GetProducts();

            var usersResponse = mapper.Map<IEnumerable<ApplicationUserDto>>(users);

            foreach (var user in usersResponse)
                user.UserProfile.IsOnline = UserHub.IsUserOnline(user.Id);

            var categoriesResponse = mapper.Map<IEnumerable<ResponseProductCaregoryDto>>(categories);
            var productsReponse = mapper.Map<IEnumerable<ResponseProductDto>>(products);

            return new ResponseAdminDto
            {
                Users = usersResponse,
                Roles = roles,
                Categories = categoriesResponse,
                Products = productsReponse
            };
        }
    }
}
