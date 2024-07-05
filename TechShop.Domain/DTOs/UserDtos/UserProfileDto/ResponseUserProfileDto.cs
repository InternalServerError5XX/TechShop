namespace TechShop.Domain.DTOs.UserDtos.UserProfileDto
{
    public class ResponseUserProfileDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
