using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class DeleteClientStatusHistoryHandler : IRequestHandler<DeleteCommand<ClientStatusHistory>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteClientStatusHistoryHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<Unit> Handle(DeleteCommand<ClientStatusHistory> request, CancellationToken cancellationToken)
    {
        var clientStatusHistory = await _context.ClientStatusHistory.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (clientStatusHistory == null)
        {
            throw new NotFoundException(typeof(ClientStatusHistory), request.Id);
        }
        if (clientStatusHistory.IsDeleted)
        {
            throw new InvalidOperationException($"ClientStatusHistory with ID {request.Id} is already deleted.");
        }

        clientStatusHistory.IsDeleted = true;
        clientStatusHistory.DeletedAt = DateTime.UtcNow;
        clientStatusHistory.DeletedUserId = _currentUser.GetUserId();
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}