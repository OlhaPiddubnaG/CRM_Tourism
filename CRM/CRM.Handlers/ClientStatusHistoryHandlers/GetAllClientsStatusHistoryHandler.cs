using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientStatusHistory;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class GetAllClientsStatusHistoryHandler : IRequestHandler<GetByIdReturnListRequest<ClientStatusHistoryResponse>, List<ClientStatusHistoryResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllClientsStatusHistoryHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<ClientStatusHistoryResponse>> Handle(GetByIdReturnListRequest<ClientStatusHistoryResponse> request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var clientId = request.Id; 

        var clientsStatusHistory = await _context.ClientStatusHistory
            .Include(csh => csh.Client)
            .Where(csh => csh.ClientId == clientId && csh.Client.CompanyId == companyId)
            .ToListAsync(cancellationToken);

        var clientStatusHistoryResponses = _mapper.Map<List<ClientStatusHistoryResponse>>(clientsStatusHistory);
        return clientStatusHistoryResponses;
    }
}