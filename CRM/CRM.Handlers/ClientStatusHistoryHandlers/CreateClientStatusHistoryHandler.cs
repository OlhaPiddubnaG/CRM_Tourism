using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.ClientStatusHistory;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class CreateClientStatusHistoryHandler : IRequestHandler<CreateClientStatusHistoryCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateClientStatusHistoryHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateClientStatusHistoryCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId &&
                                      c.CompanyId == companyId &&
                                      !c.IsDeleted, cancellationToken);

        if (client == null)
        {
            throw new UnauthorizedAccessException(
                "User is not authorized to create status history for a client from a different company.");
        }

        client.CurrentStatus = request.ClientStatus;
        var clientStatusHistory = _mapper.Map<ClientStatusHistory>(request);

        clientStatusHistory.ClientId = request.ClientId;
        clientStatusHistory.DateTime = DateTime.UtcNow;
        clientStatusHistory.ClientStatus = request.ClientStatus;
        clientStatusHistory.CreatedAt = DateTime.UtcNow;
        clientStatusHistory.CreatedUserId = _currentUser.GetUserId();
        _context.ClientStatusHistory.Add(clientStatusHistory);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(ClientStatusHistory), ex);
        }

        return new CreatedResponse(clientStatusHistory.Id);
    }
}