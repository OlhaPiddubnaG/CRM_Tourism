using AutoMapper;
using CRM.Domain.Commands.RoomType;
using CRM.Domain.Entities;
using CRM.Domain.Responses.RoomType;

namespace CRM.WebApi.Mapping;

public class RoomTypeProfile : Profile
{
    public RoomTypeProfile()
    {
        CreateMap<CreateRoomTypeCommand, RoomType>();
        CreateMap<UpdateRoomTypeCommand, RoomType>();
        CreateMap<RoomType, RoomTypeResponse>();
    }
}