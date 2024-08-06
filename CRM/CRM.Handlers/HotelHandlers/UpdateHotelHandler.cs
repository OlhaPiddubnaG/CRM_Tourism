using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Hotel;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.HotelHandlers;

public class UpdateHotelHandler : IRequestHandler<UpdateHotelCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateHotelHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var existingHotel = await _context.Hotels
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingHotel == null)
        {
            throw new NotFoundException(typeof(Hotel), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingHotel.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update hotel.");
        }

        existingHotel.RoomTypeId = request.RoomTypeId;
        existingHotel.Name = request.Name;
        existingHotel.Comment = request.Comment;
        existingHotel.UpdatedAt = DateTime.UtcNow;
        existingHotel.UpdatedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(RoomType), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}