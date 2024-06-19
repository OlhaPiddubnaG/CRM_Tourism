using AutoMapper;
using CRM.Domain.Commands.PassportInfo;
using CRM.Domain.Entities;
using CRM.Domain.Responses.PassportInfo;

namespace CRM.WebApi.Mapping;

public class PassportInfoProfile : Profile
{
    public PassportInfoProfile ()
    {
        CreateMap<CreatePassportInfoCommand, PassportInfo>();
        CreateMap<UpdatePassportInfoCommand, PassportInfo>();
        CreateMap<PassportInfo, PassportInfoResponse>();
    }
}