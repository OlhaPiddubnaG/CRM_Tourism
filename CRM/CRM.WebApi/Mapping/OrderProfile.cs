using AutoMapper;
using CRM.Domain.Commands.Order;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Order;
using CRM.Domain.Responses.Stays;

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
            .ForMember(dest => dest.CountryFromName, opt => opt.MapFrom(src => src.CountryFrom.Name))
            .ForMember(dest => dest.CountryToName, opt => opt.MapFrom(src => src.CountryTo.Name))
            .ForMember(dest => dest.Stays, opt => opt.MapFrom(src => src.Stays.Select(stay => new StaysResponse
            {
                Id = stay.Id,
                OrderId = stay.OrderId,
                HotelName = stay.Hotel.Name,
                CheckInDate = stay.CheckInDate,
                NumberOfNights = stay.NumberOfNights,
                Comment = stay.Comment
            }).ToList()))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => $"{src.Client.Name} {src.Client.Surname}"))
            .ForMember(dest => dest.LatestStatus,
                opt => opt.MapFrom(src =>
                    src.OrderStatusHistory.OrderByDescending(h => h.OrderStatus).FirstOrDefault().OrderStatus
                        .ToString()));
    }
}