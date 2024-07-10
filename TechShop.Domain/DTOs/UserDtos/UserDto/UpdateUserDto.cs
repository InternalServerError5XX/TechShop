using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.UserDtos.UserDto
{
    public class UpdateUserDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }

        [Required]
        public string RoleId { get; set; } = string.Empty;
    }
}
