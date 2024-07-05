using System.ComponentModel.DataAnnotations;
using TechShop.Domain.DTOs.ProductDtos.ProductPhoto;

namespace TechShop.Domain.DTOs.ProductDtos.ProductDto
{
    public class RequestProductDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public IEnumerable<RequestProductPhotoDto> ProductPhotos { get; set; } = [];
    }
}
