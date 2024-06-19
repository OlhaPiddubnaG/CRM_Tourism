using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.ClientPrivateData;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class UpdateClientPrivateDataHandler : IRequestHandler<UpdateClientPrivateDataCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdateClientPrivateDataHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateClientPrivateDataCommand request, CancellationToken cancellationToken)
    {
        var existingClient = await _context.ClientPrivateDatas
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingClient == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), request.Id);
        }

        existingClient.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(ClientPrivateData), ex);
        }

        return Unit.Value;
    }
}