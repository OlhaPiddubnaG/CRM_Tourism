using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Hotel;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.HotelHandlers;

public class CreateHotelHandler : IRequestHandler<CreateHotelCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateHotelHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        if (companyId != request.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create this hotel.");
        }

        var hotel = _mapper.Map<Hotel>(request);

        hotel.CreatedAt = DateTime.UtcNow;
        hotel.CreatedUserId = _currentUser.GetUserId();

        _context.Hotels.Add(hotel);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Hotel), ex);
        }

        return new CreatedResponse(hotel.Id);
    }
}