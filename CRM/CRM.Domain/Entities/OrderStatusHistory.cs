using CRM.Domain.Entities.Base;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class OrderStatusHistory : Auditable
{
    public Guid OrderId { get; set; } 
    public Order? Order { get; set; } 
    public DateTime DateTime { get; set; } 
    public OrderStatus OrderStatus { get; set; } 
}