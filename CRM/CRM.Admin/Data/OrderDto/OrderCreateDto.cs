using CRM.Admin.Data.MealsDto;
using CRM.Admin.Data.NumberOfPeopleDto;
using CRM.Admin.Data.PaymentDto;
using CRM.Admin.Data.StaysDto;
using CRM.Domain.Enums;

namespace CRM.Admin.Data.OrderDto;

public class OrderCreateDto
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public Guid TouroperatorId { get; set; }
    public DateTime? DateFrom { get; set; } = DateTime.UtcNow;
    public DateTime? DateTo { get; set; } = DateTime.UtcNow;
    public Guid CountryFromId { get; set; }
    public Guid CountryToId { get; set; }
    public int NumberOfNights { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; } = "";
    public OrderStatus LatestStatus { get; set; }
    public NumberOfPeopleCreateDto[] NumberOfPeopleCreateDto { get; set; } = Array.Empty<NumberOfPeopleCreateDto>();
    public StaysCreateDto[] StaysCreateDto { get; set; } = Array.Empty<StaysCreateDto>();
    public MealsCreateDto[] MealsCreateDto { get; set; } = Array.Empty<MealsCreateDto>();
    public PaymentCreateDto[] PaymentCreateDto { get; set; } = Array.Empty<PaymentCreateDto>();
}