namespace CRM.Admin.Data.OrderDto;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public Guid? TouroperatorId { get; set; }
    public DateOnly? DateFrom { get; set; }
    public DateOnly? DateTo { get; set; }
    public Guid CountryFromId { get; set; }
    public Guid CountryToId { get; set; }
    public int NumberOfNights { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; }
}