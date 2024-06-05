using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Authentication;

public class ResetPasswordCommand : IRequest<ResultBaseResponse>
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}