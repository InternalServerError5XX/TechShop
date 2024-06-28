using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TechShop.Domain.Entities;

namespace TechShop.Domain.DTOs.User
{
    public class ApplicationUser : IdentityUser
    {
        public UserProfile UserProfile { get; set; } = null!;
    }
}
