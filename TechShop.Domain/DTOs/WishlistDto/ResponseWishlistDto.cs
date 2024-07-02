using TechShop.Domain.DTOs.WishlistItemDto;

namespace TechShop.Domain.DTOs.WishlistDto
{
    public class ResponseWishlistDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ICollection<ResponseWishlistItemDto> WishlistItems { get; set; } = [];
    }
}
