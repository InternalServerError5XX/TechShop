using System.ComponentModel.DataAnnotations;
using TechShop.Domain.DTOs.AuthDto;

namespace TechShop.Domain.DTOs.UserDtos.UserDto
{
    public class RequestUserDto
    {
        public RegisterDto User { get; set; } = null!;

        [Required]
        public string RoleId { get; set; } = string.Empty;
    }
}
