using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.TouroperatorHandlers;

public class DeleteTouroperatorHandler : IRequestHandler<DeleteCommand<Touroperator>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteTouroperatorHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<Touroperator> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var touroperator = await _context.Touroperators.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (touroperator == null)
        {
            throw new NotFoundException(typeof(Touroperator), request.Id);
        }

        if (companyId != touroperator.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this touroperator.");
        }

        if (touroperator.IsDeleted)
        {
            throw new InvalidOperationException($"Touroperator with ID {request.Id} is already deleted.");
        }

        touroperator.IsDeleted = true;
        touroperator.DeletedAt = DateTime.UtcNow;
        touroperator.DeletedUserId = _currentUser.GetUserId();
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Touroperator), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully deleted."
        };
    }
}