using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
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
    
    public async Task<List<CountryResponse>> Handle(GetAllRequest<CountryResponse> request, CancellationToken cancellationToken)
    {
        var countries = await GetCompaniesByRoleAsync(cancellationToken);

        var countryResponses = _mapper.Map<List<CountryResponse>>(countries);
        return countryResponses;
    }

    private async Task<List<Country>> GetCompaniesByRoleAsync(CancellationToken cancellationToken)
    {
        var roles = _currentUser.GetRoles();
        if (roles.Contains(RoleType.Admin))
        {
            return await _context.Countries.ToListAsync(cancellationToken);
        }

        var companyId = _currentUser.GetCompanyId();
        return await _context.Countries.Where(c => c.Id == companyId).ToListAsync(cancellationToken);
    }
}