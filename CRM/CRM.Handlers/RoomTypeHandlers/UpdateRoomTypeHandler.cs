using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.RoomType;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.RoomTypeHandlers;

public class UpdateRoomTypeHandler : IRequestHandler<UpdateRoomTypeCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateRoomTypeHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
    {
        var existingRoomType = await _context.RoomTypes
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingRoomType == null)
        {
            throw new NotFoundException(typeof(RoomType), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingRoomType.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update roomType.");
        }

        existingRoomType.Name = request.Name;
        existingRoomType.Description = request.Description;

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