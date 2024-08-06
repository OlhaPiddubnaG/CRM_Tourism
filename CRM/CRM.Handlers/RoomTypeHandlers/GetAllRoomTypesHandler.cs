using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.RoomType;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.RoomTypeHandlers;

public class GetAllRoomTypesHandler : IRequestHandler<GetAllRequest<RoomTypeResponse>, List<RoomTypeResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllRoomTypesHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<RoomTypeResponse>> Handle(GetAllRequest<RoomTypeResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var roomTypes = await _context.RoomTypes
            .Where(r => r.CompanyId == companyId &&
                        !r.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!roomTypes.Any())
        {
            return new List<RoomTypeResponse>();
        }

        var roomTypeResponses = _mapper.Map<List<RoomTypeResponse>>(roomTypes);

        return roomTypeResponses;
    }
}