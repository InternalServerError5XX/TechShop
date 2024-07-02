using TechShop.Domain.DTOs.BasketItemDto;

namespace TechShop.Domain.DTOs.BasketDto
{
    public class ResponseBasketDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ICollection<ResponseBasketItemDto> BasketItems { get; set; } = null!;
    }
}
