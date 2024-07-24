using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Country;

public class UpdateCountryCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = null!;
}