using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.ClientStatusHistory;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientStatusHistoryHandlers;

public class UpdateClientStatusHistoryHandler  : IRequestHandler<UpdateClientStatusHistoryCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdateClientStatusHistoryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateClientStatusHistoryCommand request, CancellationToken cancellationToken)
    {
        var existingClientStatusHistory = await _context.ClientStatusHistory
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingClientStatusHistory == null)
        {
            throw new NotFoundException(typeof(ClientStatusHistory), request.Id);
        }

        existingClientStatusHistory.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(ClientStatusHistory), ex);
        }

        return Unit.Value;
    }
}