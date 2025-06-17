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
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Carts,
                           opt => opt.MapFrom(src => src.Carts)) 
                .ForMember(dest => dest.Wishlists,
                           opt => opt.MapFrom(src => src.Wishlists));

            CreateMap<Category, CategoryViewDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Product, ProductViewDto>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<Wishlist, WishlistViewDto>().ReverseMap();
                
            CreateMap<CartItems, CartItemViewDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.RealPrice, opt => opt.MapFrom(src => src.Product.RealPrice))
                .ForMember(dest => dest.OfferPrice, opt => opt.MapFrom(src => src.Product.OfferPrice));
            CreateMap<Cart, CartViewDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));


        }
    }

   
}
