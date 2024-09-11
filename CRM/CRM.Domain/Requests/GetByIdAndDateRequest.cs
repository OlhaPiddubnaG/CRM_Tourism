using MediatR;

namespace CRM.Domain.Requests;

public record struct GetByIdAndDateRequest<T>(Guid Id, DateTime Date) : IRequest<List<T>>;