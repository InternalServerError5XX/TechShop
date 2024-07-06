using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;

namespace TechShop.Domain.DTOs.AdminDto
{
    public class ResponseAdminDto
    {
        public IEnumerable<ApplicationUserDto>? Users { get; set; }
        public IEnumerable<ResponseProductDto>? Products { get; set; }
        public IEnumerable<ResponseProductCaregoryDto>? Categories { get; set; }
    }
}
