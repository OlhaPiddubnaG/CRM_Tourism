using AutoMapper;
using CRM.Domain.Entities;
using CRM.Domain.Responses.ClientStatusHistory;

namespace CRM.WebApi.Mapping;

public class ClientStatusHistoryProfile : Profile
{
    public ClientStatusHistoryProfile()
    {
        CreateMap<ClientStatusHistory, ClientStatusHistoryResponse>();
    }
}