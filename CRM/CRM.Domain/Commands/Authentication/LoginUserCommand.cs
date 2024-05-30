using CRM.Domain.Responses.Authentication;
using MediatR;

namespace CRM.Domain.Commands.Authentication;

public record LoginUserCommand : IRequest<LoginUserResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}