using AutoMapper;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Entities;
using CRM.Domain.Responses.ClientPrivateData;

namespace CRM.WebApi.Mapping;

public class ClientPrivateDataProfile : Profile
{
    public ClientPrivateDataProfile()
    {
        CreateMap<CreateClientPrivateDataCommand, ClientPrivateData>();
        CreateMap<UpdateClientPrivateDataCommand, ClientPrivateData>();
        CreateMap<ClientPrivateData, ClientPrivateDataResponse>();
    }
}