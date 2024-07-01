using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Сountry;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class GetCountryByIdHandler : IRequestHandler<GetByIdRequest<CountryResponse>, CountryResponse>
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
        var companyId = _currentUser.GetCompanyId();

        var country = await _context.Countries.FirstOrDefaultAsync(
            c => c.Id == request.Id && c.CompanyId == companyId && !c.IsDeleted, cancellationToken);
      
        if (country == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access this country.");
        }

        if (country == null)
        {
            throw new NotFoundException(typeof(Country), request.Id);
        }

        var countryResponse = _mapper.Map<CountryResponse>(country);
        return countryResponse;
    }
}