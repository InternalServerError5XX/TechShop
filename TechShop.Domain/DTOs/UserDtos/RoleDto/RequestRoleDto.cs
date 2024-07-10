using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.UserDtos.RoleDto
{
    public class RequestRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
