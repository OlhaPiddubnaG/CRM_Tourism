using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Client;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientHandlers;

public class UpdateClientHandler : IRequestHandler<UpdateClientCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdateClientHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var existingClient = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingClient == null)
        {
            throw new NotFoundException(typeof(Client), request.Id);
        }

        existingClient.Name = request.Name;
        existingClient.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Client), ex);
        }

        return Unit.Value;
    }
}