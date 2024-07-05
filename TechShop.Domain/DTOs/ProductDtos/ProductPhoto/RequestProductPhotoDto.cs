using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.ProductDtos.ProductPhoto
{
    public class RequestProductPhotoDto
    {
        [Required]
        public IFormFile Photo { get; set; } = null!;

        public int? ProductId { get; set; }
    }
}
