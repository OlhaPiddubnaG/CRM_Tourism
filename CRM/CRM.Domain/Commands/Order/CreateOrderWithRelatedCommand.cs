using CRM.Domain.Commands.Meals;
using CRM.Domain.Commands.NumberOfPeople;
using CRM.Domain.Commands.Stays;
using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Order;

public class CreateOrderWithRelatedCommand : IRequest<ResultBaseResponse>
{
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public Guid? TouroperatorId { get; set; }
    public DateTime DateFrom { get; set; } = DateTime.UtcNow;
    public DateTime DateTo { get; set; } = DateTime.UtcNow;
    public Guid CountryFromId { get; set; }
    public Guid CountryToId { get; set; }
    public int NumberOfNights { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; } = "";
    public OrderStatus LatestStatus { get; set; }
    public CreateNumberOfPeopleCommand[] NumberOfPeopleCreateDto { get; set; }
    public CreateStaysCommand[] StaysCreateDto { get; set; }
    public CreateMealsCommand[] MealsCreateDto { get; set; }
}