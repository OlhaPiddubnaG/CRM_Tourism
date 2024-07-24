using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Country;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class CreateCountryHandler : IRequestHandler<CreateCountryCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateCountryHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        if (companyId != request.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create a country.");
        }

        var existingCountry = await _context.Countries
            .FirstOrDefaultAsync(c => c.Name.ToUpper() == request.Name.ToUpper() &&
                                      c.CompanyId == request.CompanyId, cancellationToken);

        if (existingCountry != null)
        {
            throw new ExistException();
        }

        var country = _mapper.Map<Country>(request);
        _context.Countries.Add(country);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Country), ex);
        }

        return new CreatedResponse(country.Id);
    }
}