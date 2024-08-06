using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Stays;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.StaysHandlers;

public class UpdateStaysHandler : IRequestHandler<UpdateStaysCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateStaysHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateStaysCommand request,
        CancellationToken cancellationToken)
    {
        var existingStays = await _context.Stays
            .Include(o => o.Order)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingStays == null)
        {
            throw new NotFoundException(typeof(Stays), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingStays.Order.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update stays.");
        }

        existingStays.CheckInDate = request.CheckInDate;
        existingStays.NumberOfNights = request.NumberOfNights;
        existingStays.Comment = request.Comment;
        existingStays.UpdatedAt = DateTime.UtcNow;
        existingStays.UpdatedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Stays), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}