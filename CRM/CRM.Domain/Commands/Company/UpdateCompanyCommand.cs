using MediatR;

namespace CRM.Domain.Commands.Company;

public class UpdateCompanyCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
}