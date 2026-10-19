using AutoMapper;
using ECommerce.Application.DTOs.Order;
using ECommerce.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
            .ForMember(X => X.DeliveryMethod, o => o.MapFrom(X => X.DeliveryMethod.ShortName))
            .ForMember(X => X.DeliveryMethodCost, o => o.MapFrom(X => X.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(X => X.ProductId, O => O.MapFrom(S => S.Product.ProductId))
            .ForMember(X => X.ProductName, O => O.MapFrom(S => S.Product.ProductName))
            .ForMember(X => X.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
        
    }
}
