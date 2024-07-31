using AutoMapper;
using CRM.Domain.Commands.Order;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Order;

namespace CRM.WebApi.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderCommand, Order>()
            .ForMember(dest => dest.CountryFrom, opt => opt.Ignore())
            .ForMember(dest => dest.CountryTo, opt => opt.Ignore());
        CreateMap<CreateOrderWithRelatedCommand, Order>();
        CreateMap<UpdateOrderCommand, Order>();
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.LatestStatus,
                opt => opt.MapFrom(src =>
                    src.OrderStatusHistory.OrderByDescending(h => h.OrderStatus).FirstOrDefault().OrderStatus
                        .ToString()));
    }
}