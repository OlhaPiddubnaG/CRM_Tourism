using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.ClientStatusHistory;

public class CreateClientStatusHistoryCommand : IRequest<CreatedResponse>
{
    public Guid ClientId { get; set; } 
    public DateTime DateTime { get; set; } 
    public ClientStatus ClientStatus { get; set; } 
}