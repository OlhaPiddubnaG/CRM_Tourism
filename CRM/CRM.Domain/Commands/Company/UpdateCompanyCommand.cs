using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Company;

public class UpdateCompanyCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}