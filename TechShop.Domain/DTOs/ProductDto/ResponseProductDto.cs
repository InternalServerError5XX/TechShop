using TechShop.Domain.DTOs.CaregoryDto;
using TechShop.Domain.DTOs.ProductPhoto;

namespace TechShop.Domain.DTOs.ProductDto
{
    public class ResponseProductDto
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ResponseProductCaregoryDto Category { get; set; } = null!;
        public ICollection<ResponseProductPhotoDto> ProductPhotos { get; set; } = [];
    }
}
