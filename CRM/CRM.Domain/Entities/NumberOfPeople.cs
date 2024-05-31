using CRM.Domain.Entities.Base;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class NumberOfPeople : BaseEntity
{
    public Guid OrderId { get; set; } 
    public Order? Order { get; set; } 
    public int Number { get; set; }
    public ClientCategory ClientCategory { get; set; }
}