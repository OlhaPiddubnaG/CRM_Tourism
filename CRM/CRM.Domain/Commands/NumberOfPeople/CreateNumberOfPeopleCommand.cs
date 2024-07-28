using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.NumberOfPeople;

public class CreateNumberOfPeopleCommand : IRequest<CreatedResponse>
{
    public Guid OrderId { get; set; }
    public int Number { get; set; }
    public ClientCategory ClientCategory { get; set; }
}