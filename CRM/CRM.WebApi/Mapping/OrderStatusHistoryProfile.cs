using AutoMapper;
using CRM.Domain.Entities;
using CRM.Domain.Responses.OrderStatusHistory;

namespace CRM.WebApi.Mapping;

public class OrderStatusHistoryProfile : Profile
{
    public OrderStatusHistoryProfile()
    {
        CreateMap<OrderStatusHistory, OrderStatusHistoryResponse>();
    }
}