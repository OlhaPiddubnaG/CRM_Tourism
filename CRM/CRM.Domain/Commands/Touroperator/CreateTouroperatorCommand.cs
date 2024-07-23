using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Touroperator;

public class CreateTouroperatorCommand : IRequest<CreatedResponse>
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = null!;
}