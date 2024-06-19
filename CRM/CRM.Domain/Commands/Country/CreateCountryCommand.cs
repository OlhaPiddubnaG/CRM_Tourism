using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Country;

public class CreateCountryCommand : IRequest<CreatedResponse>
{
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } = null!;
}