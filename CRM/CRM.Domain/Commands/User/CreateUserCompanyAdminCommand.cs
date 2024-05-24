using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.User;

public class CreateUserCompanyAdminCommand : IRequest<CreatedResponse>
{
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
}