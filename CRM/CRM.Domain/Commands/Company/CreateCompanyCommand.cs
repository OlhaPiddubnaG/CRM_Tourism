using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Company;

public class CreateCompanyCommand : IRequest<CreatedResponse>
{
    public string Name { get; set; } 
}