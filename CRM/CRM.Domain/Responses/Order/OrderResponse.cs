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
    public Entities.Country? CountryFrom { get; set; }
    public Entities.Country? CountryTo { get; set; }
    public int NumberOfNights { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; }
    public bool IsDeleted { get; set; } 
}