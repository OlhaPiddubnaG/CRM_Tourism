using MediatR;

namespace CRM.Domain.Commands.User;

public class UpdateUserCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string Email { get; set; } 
    public string Password { get; set; } 
}