using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Hotel;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.HotelHandlers;

public class GetAllHotelsHandler : IRequestHandler<GetAllRequest<HotelResponse>, List<HotelResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllHotelsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<HotelResponse>> Handle(GetAllRequest<HotelResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var hotels = await _context.Hotels
            .Where(h => h.CompanyId == companyId &&
                        !h.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!hotels.Any())
        {
            return new List<HotelResponse>();
        }

        var hotelResponses = _mapper.Map<List<HotelResponse>>(hotels);

        return hotelResponses;
    }
}