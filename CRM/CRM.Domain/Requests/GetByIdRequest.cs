using MediatR;

namespace CRM.Domain.Requests;

public record struct GetByIdRequest<T>(Guid Id) : IRequest<T>;