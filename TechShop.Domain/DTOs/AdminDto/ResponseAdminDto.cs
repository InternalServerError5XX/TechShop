using Microsoft.AspNetCore.Identity;
using TechShop.Domain.DTOs.OrderDtos.OrderDto;
using TechShop.Domain.DTOs.OrderDtos.StatsDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.UserDtos.UserDto;

namespace TechShop.Domain.DTOs.AdminDto
{
    public class ResponseAdminDto
    {
        public IEnumerable<ApplicationUserDto> Users { get; set; } = [];
        public IEnumerable<IdentityRole> Roles { get; set; } = [];
        public IEnumerable<ResponseProductDto> Products { get; set; } = [];
        public IEnumerable<ResponseProductCaregoryDto> Categories { get; set; } = [];
        public IEnumerable<ResponseOrderDto> Orders { get; set; } = [];
        public OrdersStatsDto OrdersStats { get; set; } = null!;
    }
}
