using MediatR;

namespace CRM.Domain.Commands.Country;

public class UpdateCountryCommand : IRequest<Unit>
{
    public Guid Id { get; set; } 
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } = null!;
}