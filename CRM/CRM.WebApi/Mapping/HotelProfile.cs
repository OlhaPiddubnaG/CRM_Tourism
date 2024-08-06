using AutoMapper;
using CRM.Domain.Commands.Hotel;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Hotel;

namespace CRM.WebApi.Mapping;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<CreateHotelCommand, Hotel>();
        CreateMap<UpdateHotelCommand, Hotel>();
        CreateMap<Hotel, HotelResponse>();
    }
}