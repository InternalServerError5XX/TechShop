using TechShop.Domain.DTOs.UserDto;

namespace TechShop.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime LastLogin { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
