using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.NumberOfPeople;

public class UpdateNumberOfPeopleCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; } 
    public Guid OrderId { get; set; } 
    public int Number { get; set; }
    public ClientCategory ClientCategory { get; set; }
}