namespace TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto
{
    public class ResponseProductCaregoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
