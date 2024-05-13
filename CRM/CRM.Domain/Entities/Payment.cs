using CRM.Domain.Entities.Base;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class Payment : Auditable
{
    public Guid OrderId { get; set; } 
    public Order? Order { get; set; } 
    public DateTime DateTime { get; set; } 
    public decimal Amount { get; set; }
    public TypeOfPayment TypeOfPayment { get; set; }
}