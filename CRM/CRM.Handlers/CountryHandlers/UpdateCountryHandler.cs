using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Country;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class UpdateCountryHandler : IRequestHandler<UpdateCountryCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateCountryHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var existingCountry = await _context.Countries
            .FirstOrDefaultAsync(c => c.Id == request.Id &&
                                      c.CompanyId == request.CompanyId, cancellationToken);

        if (existingCountry == null)
        {
            throw new NotFoundException(typeof(Country), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (companyId != existingCountry.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update this country.");
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

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}