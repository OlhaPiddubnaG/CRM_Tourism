using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Authentication;

public class ForgotPasswordCommand : IRequest<ResultBaseResponse>
{
    public string Email { get; set; }
}