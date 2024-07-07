using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.ProductDtos.ProductDto
{
    public class CreateProductDto
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

        public IEnumerable<IFormFile> ProductPhotos { get; set; } = [];
    }
}
