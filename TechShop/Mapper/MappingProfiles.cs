using AutoMapper;
using TechShop.Domain.DTOs.AuthDto;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.BasketDtos.BasketItemDto;
using TechShop.Domain.DTOs.PaginationDto;
using TechShop.Domain.DTOs.ProductDtos.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDtos.ProductDto;
using TechShop.Domain.DTOs.ProductDtos.ProductPhoto;
using TechShop.Domain.DTOs.UserDtos.UserDto;
using TechShop.Domain.DTOs.UserDtos.UserProfileDto;
using TechShop.Domain.DTOs.WishlistDtos.WishlistDto;
using TechShop.Domain.DTOs.WishlistDtos.WishlistItemDto;
using TechShop.Domain.Entities;
using TechShop.Domain.Entities.BasketEntities;
using TechShop.Domain.Entities.ProductEntities;
using TechShop.Domain.Entities.WishlistEntities;

namespace TechShop.Infrastructure.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            var currentDate = DateTime.Now;

            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.Substring(0, src.Email.IndexOf('@'))));

            CreateMap<RegisterDto, UserProfile>()
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.LastLogin = DateTime.Now;
                });

            CreateMap<RegisterDto, LoginDto>();

            CreateMap<RequestUserProfileDto, UserProfile>()
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.LastLogin = DateTime.Now;
                });
            CreateMap<UserProfile, ResponseUserProfileDto>();
            CreateMap<ResponseUserProfileDto, UserProfile>();

            CreateMap<ApplicationUser, ApplicationUserDto>();

            CreateMap<RequestProductDto, Product>();
            CreateMap<Product, ResponseProductDto>();

            CreateMap<RequestProductPhotoDto, ProductPhoto>();
            CreateMap<ProductPhoto, ResponseProductPhotoDto>();

            CreateMap<RequestProductCategoryDto, ProductCategory>();
            CreateMap<ProductCategory, ResponseProductCaregoryDto>();

            CreateMap<RequestBasketDto, Basket>();
            CreateMap<Basket, ResponseBasketDto>();

            CreateMap<RequestBasketItemDto, BasketItem>();
            CreateMap<BasketItem, ResponseBasketItemDto>();

            CreateMap<RequestWishlistDto, Wishlist>();
            CreateMap<Wishlist, ResponseWishlistDto>();

            CreateMap<RequestWishlistItemDto, WishlistItem>();
            CreateMap<WishlistItem, ResponseWishlistItemDto>();

            CreateMap<ResponsePaginationDto<Product>, ResponsePaginationDto<ResponseProductDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
        }
    }
}
