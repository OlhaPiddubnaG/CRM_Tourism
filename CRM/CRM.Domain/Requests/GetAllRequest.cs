using MediatR;

namespace CRM.Domain.Requests;

public record GetAllRequest<T> : IRequest<List<T>>;