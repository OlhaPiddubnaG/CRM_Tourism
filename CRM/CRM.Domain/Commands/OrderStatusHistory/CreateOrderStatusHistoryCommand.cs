using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.OrderStatusHistory;

public class CreateOrderStatusHistoryCommand : IRequest<CreatedResponse>
{
    public Guid OrderId { get; set; }
    public DateTime DateTime { get; set; }
    public OrderStatus OrderStatus { get; set; }
}