using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.PaginationDto
{
    public class RequestPaginationDto
    {
        [Required]
        public int PageNumber { get; set; } = 1;

        [Required]
        public int PageSize { get; set; } = 20;
    }
}
