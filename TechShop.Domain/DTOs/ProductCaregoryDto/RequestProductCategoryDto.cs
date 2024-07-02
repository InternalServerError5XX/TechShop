using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.ProductCaregoryDto
{
    public class RequestProductCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
