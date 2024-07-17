using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.ClientPrivateDataHandlers;

public class DeleteClientPrivateDataHandler : IRequestHandler<DeleteCommand<ClientPrivateData>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteClientPrivateDataHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    
    public async Task<Unit> Handle(DeleteCommand<ClientPrivateData> request, CancellationToken cancellationToken)
    {
        var currentUserCompanyId = _currentUser.GetCompanyId();
        
        var clientPrivateData = await _context.ClientPrivateDatas
            .Include(cpd => cpd.Client)
            .FirstOrDefaultAsync(cpd => cpd.Id == request.Id, cancellationToken);
            
        if (clientPrivateData == null)
        {
            throw new NotFoundException(typeof(ClientPrivateData), request.Id);
        }
        
        if (currentUserCompanyId != clientPrivateData.Client?.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this client private data.");
        }
        
        if (clientPrivateData.IsDeleted)
        {
            throw new InvalidOperationException($"ClientPrivateData with ID {request.Id} is already deleted.");
        }

        clientPrivateData.IsDeleted = true;
        clientPrivateData.DeletedAt = DateTime.UtcNow;
        clientPrivateData.DeletedUserId = _currentUser.GetUserId();
        await _context.SaveChangesAsync(cancellationToken);

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