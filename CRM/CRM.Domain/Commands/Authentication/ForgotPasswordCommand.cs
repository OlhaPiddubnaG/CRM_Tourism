using MediatR;

namespace CRM.Domain.Commands.Authentication;

public class ForgotPasswordCommand : IRequest<Unit>
{
    public string Email { get; set; }
}