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
        CreateMap<Client, ClientResponse>()
            .ForMember(d => d.ManagerNames, 
                opt => opt.MapFrom(src => src.Users.Select(u => u.Name).ToList()))
            .ForMember(dest => dest.LatestStatus,
                opt => opt.MapFrom(src =>
                    src.ClientStatusHistory.OrderByDescending(h => h.ClientStatus).FirstOrDefault().ClientStatus
                        .ToString()));
    }
}