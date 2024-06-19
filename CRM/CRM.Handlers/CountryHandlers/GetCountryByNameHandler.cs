using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Сountry;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.CountryHandlers;

public class GetCountryByNameHandler : IRequestHandler<GetByNameRequest<CountryResponse>, CountryResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetCountryByNameHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CountryResponse> Handle(GetByNameRequest<CountryResponse> request, CancellationToken cancellationToken)
    {
        var country = await _context.Countries
            .FirstOrDefaultAsync(c => c.Name == request.Name.ToUpper(), cancellationToken);

        if (country == null)
        {
            return null;
        }

        var countryResponse = _mapper.Map<CountryResponse>(country);
        return countryResponse;
    }
}
