using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class Stays : Auditable
{
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public DateTime CheckInDate { get; set; }
    public int NumberOfNights { get; set; }
    public string Comment { get; set; }
}