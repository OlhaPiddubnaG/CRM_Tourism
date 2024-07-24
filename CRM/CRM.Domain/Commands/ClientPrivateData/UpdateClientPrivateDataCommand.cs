using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.ClientPrivateData;

public class UpdateClientPrivateDataCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
}