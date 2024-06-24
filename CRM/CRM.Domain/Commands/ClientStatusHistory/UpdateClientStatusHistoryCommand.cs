using CRM.Domain.Enums;
using MediatR;

namespace CRM.Domain.Commands.ClientStatusHistory;

public class UpdateClientStatusHistoryCommand : IRequest<Unit>
{
    public Guid Id { get; set; } 
    public Guid ClientId { get; set; } 
    public DateTime DateTime { get; set; } 
    public ClientStatus ClientStatus { get; set; } 
}