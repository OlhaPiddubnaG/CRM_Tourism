using MediatR;

namespace CRM.Domain.Requests;

public record struct GetByIdReturnListRequest<T>(Guid Id) : IRequest<List<T>>;