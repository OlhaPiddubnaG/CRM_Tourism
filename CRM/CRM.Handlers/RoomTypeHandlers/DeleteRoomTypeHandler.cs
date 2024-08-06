using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.RoomTypeHandlers;

public class DeleteRoomTypeHandler : IRequestHandler<DeleteCommand<RoomType>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteRoomTypeHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<RoomType> request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var roomType = await _context.RoomTypes.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (roomType == null)
        {
            throw new NotFoundException(typeof(RoomType), request.Id);
        }

        if (companyId != roomType.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this roomType.");
        }

        if (roomType.IsDeleted)
        {
            throw new InvalidOperationException($"RoomType with ID {request.Id} is already deleted.");
        }

        roomType.IsDeleted = true;

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
            Message = "Successfully deleted."
        };
    }
}