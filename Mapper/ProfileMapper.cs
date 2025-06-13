using AutoMapper;
using Foodkart.DTOs.AddDto;
using Foodkart.DTOs.Auth;
using Foodkart.DTOs.Products;
using Foodkart.DTOs.ViewDto;
using Foodkart.Models.Entities.Carts;
using Foodkart.Models.Entities.Main;
using Foodkart.Models.Entities.Orders;
using Foodkart.Models.Entities.Products;

namespace Foodkart.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<User,LoginDto>().ReverseMap();
            CreateMap<User, RegistrationDto>().ReverseMap();
            CreateMap<User, UserDto>().
                ForMember(dest => dest.Carts,
                    opt => opt.MapFrom(src => src.Carts))
                .ForMember(dest => dest.Wishlists,
                opt => opt.MapFrom(src => src.Wishlists.Count));
            CreateMap<Category, CategoryViewDto>().ReverseMap();
            CreateMap<Product, ProductViewDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CartItems, CartItemViewDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(target => target.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(target => target.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(target => target.Image, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(target => target.RealPrice, opt => opt.MapFrom(src => src.Product.RealPrice))
                .ForMember(target => target.OfferPrice, opt => opt.MapFrom(src => src.Product.OfferPrice))
                .ForMember(target => target.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
            CreateMap<Cart, CartViewDto>().ForMember(dest => dest.Items,opt => opt.MapFrom(src=>src.CartItems));


        }
    }

    
}
