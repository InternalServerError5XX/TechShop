﻿using System.ComponentModel.DataAnnotations;

namespace TechShop.Domain.DTOs.UserProfileDto
{
    public class RequestUserProfileDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; }
    }
}
