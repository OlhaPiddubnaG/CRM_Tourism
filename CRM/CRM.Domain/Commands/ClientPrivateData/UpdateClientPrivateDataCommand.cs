using MediatR;

namespace CRM.Domain.Commands.ClientPrivateData;

public class UpdateClientPrivateDataCommand : IRequest<Unit>
{
    public Guid Id { get; set; } 
    public Guid ClientId { get; set; } 
}