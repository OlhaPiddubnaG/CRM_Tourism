using CRM.Domain.Responses.Authentication;
using MediatR;

namespace CRM.Domain.Commands.Authentication;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResponse>;



