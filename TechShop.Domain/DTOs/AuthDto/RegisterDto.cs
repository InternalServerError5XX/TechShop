using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.AuthDto
{
    public class RegisterDto
    {
        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        [Range(12, 120, ErrorMessage = "Age must be between 12 and 120")]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
