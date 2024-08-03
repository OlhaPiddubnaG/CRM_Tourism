using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.HotelHandlers;

public class DeleteHotelHandler : IRequestHandler<DeleteCommand<Hotel>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteHotelHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<Hotel> request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var hotel = await _context.Hotels.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (hotel == null)
        {
            throw new NotFoundException(typeof(Hotel), request.Id);
        }

        if (companyId != hotel.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this hotel.");
        }

        if (hotel.IsDeleted)
        {
            throw new InvalidOperationException($"Hotel with ID {request.Id} is already deleted.");
        }

        hotel.IsDeleted = true;
        hotel.DeletedAt = DateTime.UtcNow;
        hotel.DeletedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Hotel), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully deleted."
        };
    }
}