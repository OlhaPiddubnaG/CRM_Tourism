using MediatR;

namespace CRM.Domain.Commands;

public record struct DeleteCommand<T>(Guid Id) : IRequest<Unit>;