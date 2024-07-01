using AutoMapper;
using CRM.Domain.Commands.Client;
using CRM.Domain.Entities;
using CRM.Domain.Responses.Client;

namespace CRM.WebApi.Mapping;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<CreateClientCommand, Client>();
        CreateMap<CreateClientWithRelatedCommand, Client>();
        CreateMap<UpdateClientCommand, Client>();
        CreateMap<Client, ClientResponse>();
    }
}