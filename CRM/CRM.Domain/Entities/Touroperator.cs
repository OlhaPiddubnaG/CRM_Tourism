using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class Touroperator : BaseEntity
{
    public Guid OrderId { get; set; } 
    public Order? Order { get; set; } 
    public string Name { get; set; } = null!;
}