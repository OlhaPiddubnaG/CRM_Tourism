using MediatR;

namespace CRM.Domain.Requests;

public record struct GetByEmailRequest<T>(string Email) : IRequest<T>;