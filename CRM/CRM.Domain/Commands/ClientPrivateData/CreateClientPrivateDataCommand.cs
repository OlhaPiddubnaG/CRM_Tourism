using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.ClientPrivateData;

public class CreateClientPrivateDataCommand : IRequest<CreatedResponse>
{
    public Guid ClientId { get; set; } 
}