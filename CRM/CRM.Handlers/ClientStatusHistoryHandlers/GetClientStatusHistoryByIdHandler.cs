using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.ClientStatusHistory;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class GetClientStatusHistoryByIdHandler : IRequestHandler<GetByIdRequest<ClientStatusHistoryResponse>, ClientStatusHistoryResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetClientStatusHistoryByIdHandler(AppDbContext context, IMapper mapper,ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }
    
    public async Task<ClientStatusHistoryResponse> Handle(GetByIdRequest<ClientStatusHistoryResponse> request,
        CancellationToken cancellationToken)
    {
        var clientStatusHistory = await _context.ClientStatusHistory
            .Include(csh => csh.Client)
            .FirstOrDefaultAsync(csh => csh.Id == request.Id, cancellationToken);

        if (clientStatusHistory == null)
        {
            throw new NotFoundException(typeof(ClientStatusHistory), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (clientStatusHistory.Client?.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to access status history for a client from a different company.");
        }

        var clientStatusHistoryResponse = _mapper.Map<ClientStatusHistoryResponse>(clientStatusHistory);
        return clientStatusHistoryResponse;
    }
}