using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class DeleteCountryHandler : IRequestHandler<DeleteCommand<Country>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteCountryHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteCommand<Country> request, CancellationToken cancellationToken)
    {
        var currentUserCompanyId = _currentUser.GetCompanyId();

        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

        if (country == null)
        {
            throw new NotFoundException(typeof(Country), request.Id);
        }

        if (currentUserCompanyId != country.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this country.");
        }

        if (country.IsDeleted)
        {
            throw new InvalidOperationException($"Country with ID {request.Id} is already deleted.");
        }

        country.IsDeleted = true;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Country), ex);
        }

        return Unit.Value;
    }
}