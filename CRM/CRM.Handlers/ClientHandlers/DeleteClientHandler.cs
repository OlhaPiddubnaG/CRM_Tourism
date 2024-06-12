using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientHandlers;

public class DeleteClientHandler : IRequestHandler<DeleteCommand<Client>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteClientHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<Unit> Handle(DeleteCommand<Client> request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (client == null)
        {
            throw new NotFoundException(typeof(Client), request.Id);
        }
        if (client.IsDeleted)
        {
            throw new InvalidOperationException($"Client with ID {request.Id} is already deleted.");
        }

        client.IsDeleted = true;
        client.DeletedAt = DateTime.UtcNow;
        client.DeletedUserId = _currentUser.GetUserId();
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}