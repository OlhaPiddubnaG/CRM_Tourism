using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Country;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class UpdateCountryHandler: IRequestHandler<UpdateCountryCommand, Unit>
{
    private readonly AppDbContext _context;

    public UpdateCountryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var existingCountry = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingCountry == null)
        {
            throw new NotFoundException(typeof(Country), request.Id);
        }

        existingCountry.Name = request.Name;

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