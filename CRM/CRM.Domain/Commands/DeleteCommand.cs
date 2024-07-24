using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands;

public record struct DeleteCommand<T>(Guid Id) : IRequest<ResultBaseResponse>;