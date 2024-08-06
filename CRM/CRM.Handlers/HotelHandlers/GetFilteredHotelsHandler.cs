using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Hotel;
using CRM.Handlers.Services.CurrentUser;
using MediatR;

namespace CRM.Handlers.HotelHandlers;

public class GetFilteredHotelsHandler : IRequestHandler<GetFilteredAllRequest<HotelResponse>, List<HotelResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetFilteredHotelsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<HotelResponse>> Handle(GetFilteredAllRequest<HotelResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        IQueryable<Hotel> query = _context.Hotels
            .Where(h => h.CompanyId == companyId &&
                        !h.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.ToLower();
            query = query.Where(h =>
                h.Name.ToLower().Contains(searchString));
        }

        var hotelResponses = _mapper.Map<List<HotelResponse>>(query);
        return hotelResponses;
    }
}