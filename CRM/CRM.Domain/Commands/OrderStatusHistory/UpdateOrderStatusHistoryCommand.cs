using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.OrderStatusHistory;

public class UpdateOrderStatusHistoryCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; } 
    public DateTime DateTime { get; set; } 
    public OrderStatus OrderStatus { get; set; } 
}