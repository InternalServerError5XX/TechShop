﻿using TechShop.Domain.DTOs.ProductDtos.ProductDto;

namespace TechShop.Domain.DTOs.WishlistDtos.WishlistItemDto
{
    public class ResponseWishlistItemDto
    {
        public int Id { get; set; }
        public int WishlistId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ResponseProductDto Product { get; set; } = null!;
    }
}
