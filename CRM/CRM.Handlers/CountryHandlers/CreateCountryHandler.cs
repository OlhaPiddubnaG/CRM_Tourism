using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Country;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class CreateCountryHandler : IRequestHandler<CreateCountryCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CreateCountryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatedResponse> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        request.Name = request.Name.ToUpper();
        var existingCountry =
            await _context.Countries.FirstOrDefaultAsync(c => c.Name == request.Name, cancellationToken);

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
