using AutoMapper;
using CRM.Domain.Commands.OrderStatusHistory;
using CRM.Domain.Entities;
using CRM.Domain.Responses.OrderStatusHistory;

namespace CRM.WebApi.Mapping;

public class OrderStatusHistoryProfile : Profile
{
    public OrderStatusHistoryProfile()
    {
        CreateMap<CreateOrderStatusHistoryCommand, OrderStatusHistory>();
        CreateMap<UpdateOrderStatusHistoryCommand, OrderStatusHistory>();
        CreateMap<OrderStatusHistory, OrderStatusHistoryResponse>();
    }
}