
using AutoMapper;
using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;

namespace BetCommerce.API.MappingProfiles
{
    public class MainMappingProfile : Profile
    {
        public MainMappingProfile()
        {
            CreateMap<ProductRequest, Product>();
            CreateMap<OrderRequest, Order>();
            CreateMap<OrderItemRequest, OrderItem>();
        }
    }
}
