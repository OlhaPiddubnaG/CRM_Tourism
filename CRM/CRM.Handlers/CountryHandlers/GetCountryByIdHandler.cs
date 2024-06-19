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

public class GetCountryByIdHandler: IRequestHandler<GetByIdRequest<CountryResponse>, CountryResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetCountryByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CountryResponse> Handle(GetByIdRequest<CountryResponse> request,
        CancellationToken cancellationToken)
    {
        var country = await GetCompanyByIdAsync(request.Id, cancellationToken);

        if (country == null)
        {
            throw new NotFoundException(typeof(Country), request.Id);
        }

        var countryResponse = _mapper.Map<CountryResponse>(country);
        return countryResponse;
    }

    private async Task<Country> GetCompanyByIdAsync(Guid companyId, CancellationToken cancellationToken)
    {
        var roles = _currentUser.GetRoles();
        if (roles.Contains(RoleType.Admin))
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);
        }

        var currentCompanyId = _currentUser.GetCompanyId();
        if (companyId != currentCompanyId)
        {
            throw new ForbiddenException();
        }

        return await _context.Countries.FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);
    }
}
