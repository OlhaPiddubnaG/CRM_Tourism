using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Сountry;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class GetCountryByNameHandler : IRequestHandler<GetByNameRequest<CountryResponse>, CountryResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetCountryByNameHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CountryResponse> Handle(GetByNameRequest<CountryResponse> request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Name.ToUpper() == request.Name.ToUpper() && !c.IsDeleted, cancellationToken);

        if (country == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access this country.");
        }

        if (country == null)
        {
            throw new NotFoundException(typeof(Country));
        }

        var countryResponse = _mapper.Map<CountryResponse>(country);
        return countryResponse;
    }
}
