using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientStatusHistory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class GetClientStatusHistoryByIdHandler : IRequestHandler<GetByIdRequest<ClientStatusHistoryResponse>, ClientStatusHistoryResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetClientStatusHistoryByIdHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<ClientStatusHistoryResponse> Handle(GetByIdRequest<ClientStatusHistoryResponse> request,
        CancellationToken cancellationToken)
    {
        var clientStatusHistory =  await _context.ClientStatusHistory.FirstAsync(c => c.Id == request.Id , cancellationToken);

        if (clientStatusHistory == null)
        {
            throw new NotFoundException(typeof(ClientStatusHistory), request.Id);
        }

        var clientStatusHistoryResponse = _mapper.Map<ClientStatusHistoryResponse>(clientStatusHistory);
        return clientStatusHistoryResponse;
    }
}