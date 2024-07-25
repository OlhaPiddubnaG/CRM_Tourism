using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.StaysHandlers;

public class DeleteStaysHandler : IRequestHandler<DeleteCommand<Stays>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteStaysHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<Stays> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var stays = await _context.Stays
            .Include(s => s.Order)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (stays == null)
        {
            throw new NotFoundException(typeof(Stays), request.Id);
        }

        if (companyId != stays.Order.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this stays.");
        }

        if (stays.IsDeleted)
        {
            throw new InvalidOperationException($"Stays with ID {request.Id} is already deleted.");
        }

        stays.IsDeleted = true;
        stays.DeletedAt = DateTime.UtcNow;
        stays.DeletedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Stays), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully deleted."
        };
    }
}