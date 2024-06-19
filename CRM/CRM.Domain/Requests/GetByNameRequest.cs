using MediatR;

namespace CRM.Domain.Requests;

public record struct GetByNameRequest<T>(string Name) : IRequest<T>;

