using AutoMapper;
using Foodkart.DTOs.AddDto;
using Foodkart.DTOs.Auth;
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
            CreateMap<ProductDto, Product>().ReverseMap();
            
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Cart, CartViewDto>().ReverseMap();


        }
    }

    
}
