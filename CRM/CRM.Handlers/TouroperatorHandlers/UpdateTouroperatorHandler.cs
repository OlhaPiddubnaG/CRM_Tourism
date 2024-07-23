using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Touroperator;
using CRM.Domain.Entities;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.TouroperatorHandlers;

public class UpdateTouroperatorHandler : IRequestHandler<UpdateTouroperatorCommand, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateTouroperatorHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(UpdateTouroperatorCommand request, CancellationToken cancellationToken)
    {
        var existingTouroperator = await _context.Touroperators
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingTouroperator == null)
        {
            throw new NotFoundException(typeof(Touroperator), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingTouroperator.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update touroperator.");
        }

        existingTouroperator.Name = request.Name;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Touroperator), ex);
        }

        return Unit.Value;
    }
}