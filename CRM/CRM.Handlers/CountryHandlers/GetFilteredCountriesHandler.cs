using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Сountry;
using CRM.Handlers.Services.CurrentUser;
using MediatR;

namespace CRM.Handlers.CountryHandlers;

public class
    GetFilteredCountriesHandler : IRequestHandler<GetFilteredAllRequest<CountryResponse>, List<CountryResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetFilteredCountriesHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<CountryResponse>> Handle(GetFilteredAllRequest<CountryResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        IQueryable<Country> query = _context.Countries
            .Where(c => c.CompanyId == companyId &&
                        !c.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.ToLower();
            query = query.Where(c =>
                c.Name.ToLower().Contains(searchString));
        }

        var countryResponses = _mapper.Map<List<CountryResponse>>(query);
        return countryResponses;
    }
}