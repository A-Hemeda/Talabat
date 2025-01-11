using AutoMapper;
using Talabat.APIS.DTOs;
using Talabat.APIS.Helpers;
using Talabat.Core.Models;
using Address = Talabat.Core.Models.Address;
using Talabat.Core.Order_Aggregate;
using OrderAddress = Talabat.Core.Order_Aggregate.Address; // Alias  Name

namespace Talabat.APIS.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(t => t.ProductType, o => o.MapFrom(n => n.ProductType.Name))
                .ForMember(b => b.ProductBrand, o => o.MapFrom(n => n.ProductBrand.Name))
                .ForMember(dto => dto.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<AddressDto, OrderAddress>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dto => dto.DeliveryMethod, order => order.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(dto => dto.DeliveryMethodCost, order => order.MapFrom(o => o.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dto => dto.ProductId, order => order.MapFrom(o => o.Product.ProductId))
                .ForMember(dto => dto.ProductName, order => order.MapFrom(o => o.Product.ProductName))
                .ForMember(dto => dto.PictureUrl, order => order.MapFrom<OrderPictureUrlResolver>());

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        }
    }
}
