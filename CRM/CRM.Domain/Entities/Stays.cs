using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class Stays : Auditable
{
    public Guid OrderId { get; set; } 
    public Order? Order { get; set; } 
    public string Name { get; set; } = null!;
    public DateTime CheckInDate { get; set; }
    public int NumberOfNights { get; set; }
    public string Comment { get; set; }
    public List<Meals> Meals { get; set; } = new();
}