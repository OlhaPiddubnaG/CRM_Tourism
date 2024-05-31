using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.User;

public class CreateUserCommand : IRequest<CreatedResponse>
{
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
    public ICollection<RoleType> RoleTypes { get; set; }
}