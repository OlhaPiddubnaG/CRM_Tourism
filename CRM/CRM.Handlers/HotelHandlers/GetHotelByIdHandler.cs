using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Hotel;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.HotelHandlers;

public class GetHotelByIdHandler : IRequestHandler<GetByIdRequest<HotelResponse>, HotelResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetHotelByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<HotelResponse> Handle(GetByIdRequest<HotelResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var hotel = await _context.Hotels
            .FirstOrDefaultAsync(o => o.Id == request.Id &&
                                      o.CompanyId == companyId &&
                                      !o.IsDeleted);

        if (hotel == null)
        {
            throw new NotFoundException(typeof(Hotel), request.Id);
        }

        var hotelResponse = _mapper.Map<HotelResponse>(hotel);

        return hotelResponse;
    }
}