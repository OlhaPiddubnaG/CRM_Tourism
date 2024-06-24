using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientStatusHistory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class GetAllClientsStatusHistoryHandler : IRequestHandler<GetAllRequest<ClientStatusHistoryResponse>, List<ClientStatusHistoryResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetAllClientsStatusHistoryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ClientStatusHistoryResponse>> Handle(GetAllRequest<ClientStatusHistoryResponse> request,
        CancellationToken cancellationToken)
    {
        var clientsStatusHistory = await _context.ClientStatusHistory.ToListAsync(cancellationToken);

        var clientStatusHistoryResponses = _mapper.Map<List<ClientStatusHistoryResponse>>(clientsStatusHistory);
        return clientStatusHistoryResponses;
    }
}