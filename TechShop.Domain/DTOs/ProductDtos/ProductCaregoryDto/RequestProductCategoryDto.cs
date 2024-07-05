using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto
{
    public class RequestProductCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
