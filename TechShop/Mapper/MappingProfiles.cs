using AutoMapper;
using TechShop.Domain.DTOs.AuthDto;
using TechShop.Domain.DTOs.BasketDtos.BasketDto;
using TechShop.Domain.DTOs.BasketDtos.BasketItemDto;
using TechShop.Domain.DTOs.CaregoryDto;
using TechShop.Domain.DTOs.PaginationDto;
using TechShop.Domain.DTOs.ProductCaregoryDto;
using TechShop.Domain.DTOs.ProductDto;
using TechShop.Domain.DTOs.ProductPhoto;
using TechShop.Domain.DTOs.UserDto;
using TechShop.Domain.DTOs.UserProfileDto;
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
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                    dest.LastLogin = currentDate;
                });

            CreateMap<RegisterDto, LoginDto>();

            CreateMap<RequestUserProfileDto, UserProfile>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastLogin, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                    dest.LastLogin = currentDate;
                });

            CreateMap<UserProfile, ResponseUserProfileDto>();

            CreateMap<ApplicationUser, ApplicationUserDto>();

            CreateMap<RequestProductDto, Product>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                });

            CreateMap<Product, ResponseProductDto>();

            CreateMap<RequestProductPhotoDto, ProductPhoto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                });
            CreateMap<ProductPhoto, ResponseProductPhotoDto>();

            CreateMap<RequestProductCategoryDto, ProductCategory>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                });

            CreateMap<ProductCategory, ResponseProductCaregoryDto>();

            CreateMap<RequestBasketDto, Basket>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                });

            CreateMap<Basket, ResponseBasketDto>();

            CreateMap<RequestBasketItemDto, BasketItem>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                });

            CreateMap<BasketItem, ResponseBasketItemDto>();

            CreateMap<RequestWishlistDto, Wishlist>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                });

            CreateMap<Wishlist, ResponseWishlistDto>();

            CreateMap<RequestWishlistItemDto, WishlistItem>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var currentDate = DateTime.Now;
                    dest.CreatedDate = currentDate;
                    dest.UpdatedDate = currentDate;
                });

            CreateMap<WishlistItem, ResponseWishlistItemDto>();

            CreateMap<ResponsePaginationDto<Product>, ResponsePaginationDto<ResponseProductDto>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
        }
    }
}
