using CRM.Domain.Enums;

namespace CRM.Domain.Responses.OrderStatusHistory;

public class OrderStatusHistoryResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public DateTime DateTime { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public bool IsDeleted { get; set; }
}