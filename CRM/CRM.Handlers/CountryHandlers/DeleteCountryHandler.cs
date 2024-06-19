using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class DeleteCountryHandler : IRequestHandler<DeleteCommand<Country>, Unit>
{
    private readonly AppDbContext _context;

    public DeleteCountryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCommand<Country> request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (country == null)
        {
            throw new NotFoundException(typeof(Country), request.Id);
        }
        if (country.IsDeleted)
        {
            throw new InvalidOperationException($"Country with ID {request.Id} is already deleted.");
        }

        country.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}