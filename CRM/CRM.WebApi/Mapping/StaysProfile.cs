using AutoMapper;
using CRM.Domain.Commands.Stays;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Stays;

namespace CRM.WebApi.Mapping;

public class StaysProfile : Profile
{
    public StaysProfile()
    {
        CreateMap<CreateStaysCommand, Stays>();
        CreateMap<UpdateStaysCommand, Stays>();
        CreateMap<Stays, StaysResponse>();
    }
}