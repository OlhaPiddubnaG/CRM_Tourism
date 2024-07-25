using CRM.Domain.Enums;

namespace CRM.Admin.Data.OrderStatusHistoryDto;

public class OrderStatusHistoryUpdateDto
{
    public Guid Id { get; set; } 
    public Guid OrderId { get; set; } 
    public DateTime DateTime { get; set; } 
    public OrderStatus OrderStatus { get; set; } 
}