using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Сountry;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class GetAllCountriesHandler : IRequestHandler<GetAllRequest<CountryResponse>, List<CountryResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllCountriesHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<CountryResponse>> Handle(GetAllRequest<CountryResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var countries = await _context.Countries
            .Where(c => c.CompanyId == companyId && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
        
        if (countries == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access this country.");
        }

        var countryResponses = _mapper.Map<List<CountryResponse>>(countries);
        return countryResponses;
    }
}