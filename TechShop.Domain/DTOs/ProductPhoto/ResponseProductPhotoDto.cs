namespace TechShop.Domain.DTOs.ProductPhoto
{
    public class ResponseProductPhotoDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Path { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
