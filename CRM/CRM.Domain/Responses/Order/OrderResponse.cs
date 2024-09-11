using CRM.Domain.Enums;
using CRM.Domain.Responses.Stays;

namespace CRM.Domain.Responses.Order;

public class OrderResponse
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public Guid? TouroperatorId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public string?  CountryFromName { get; set; }
    public string?  CountryToName { get; set; }
    public int NumberOfNights { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; }
    public OrderStatus? LatestStatus { get; set; }
    public List<StaysResponse> Stays { get; set; } = new();
    public string ClientName { get; set; } 
    public DateTime CreatedAt { get; set; }
}