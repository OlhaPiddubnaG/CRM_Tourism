using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.RoomType;
using CRM.Handlers.Services.CurrentUser;
using MediatR;

namespace CRM.Handlers.RoomTypeHandlers;

public class
    GetFilteredRoomTypesHandler : IRequestHandler<GetFilteredAllRequest<RoomTypeResponse>, List<RoomTypeResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetFilteredRoomTypesHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<RoomTypeResponse>> Handle(GetFilteredAllRequest<RoomTypeResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        IQueryable<RoomType> query = _context.RoomTypes
            .Where(t => t.CompanyId == companyId &&
                        !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            var searchString = request.SearchString.ToLower();
            query = query.Where(t =>
                t.Name.ToLower().Contains(searchString));
        }

        var roomTypeResponses = _mapper.Map<List<RoomTypeResponse>>(query);
        return roomTypeResponses;
    }
}