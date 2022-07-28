
using AutoMapper;
using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;
using BetCommerce.Entity.Core.Responses;

namespace BetCommerce.API.MappingProfiles
{
    public class MainMappingProfile : Profile
    {
        public MainMappingProfile()
        {
            CreateMap<ProductRequest, Product>();
            CreateMap<OrderRequest, Order>();
            CreateMap<OrderItemRequest, OrderItem>();
            CreateMap<UserAccount, UserIdentityResponse>();
            CreateMap<SignUpRequest, UserAccount>();
        }
    }
}
