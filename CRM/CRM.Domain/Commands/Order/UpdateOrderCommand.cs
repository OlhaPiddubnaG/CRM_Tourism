using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Order;

public class UpdateOrderCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public Guid? TouroperatorId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public Guid CountryFromId { get; set; }
    public Guid CountryToId { get; set; }
    public int NumberOfNights { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; }
    public OrderStatus LatestStatus { get; set; } 
}