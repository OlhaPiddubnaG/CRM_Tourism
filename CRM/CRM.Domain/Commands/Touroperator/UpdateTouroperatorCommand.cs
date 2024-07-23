using MediatR;

namespace CRM.Domain.Commands.Touroperator;

public class UpdateTouroperatorCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public Guid? CompanyId { get; set; }
    public string Name { get; set; } = null!;
}