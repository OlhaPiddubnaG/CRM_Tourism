using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Order;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderHandlers;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateOrderHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var existingOrder = await _context.Orders
            .Include(o => o.NumberOfPeople)
            .Include(o => o.CountryFrom)
            .Include(o => o.CountryTo)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingOrder == null)
        {
            throw new NotFoundException(typeof(Order), request.Id);
        }

        var currentUserCompanyId = _currentUser.GetCompanyId();

        if (existingOrder.CompanyId != currentUserCompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update order.");
        }

        var countryFrom = await _context.Countries.FindAsync(request.CountryFromId);
        var countryTo = await _context.Countries.FindAsync(request.CountryToId);

        if (countryFrom == null || countryTo == null)
        {
            throw new KeyNotFoundException("Country not found.");
        }

        existingOrder.DateFrom = request.DateFrom;
        existingOrder.DateTo = request.DateTo;
        existingOrder.CountryFrom = countryFrom;
        existingOrder.CountryTo = countryTo;
        existingOrder.NumberOfNights = request.NumberOfNights;
        existingOrder.Amount = request.Amount;
        existingOrder.Comment = request.Comment;
        existingOrder.UpdatedAt = DateTime.UtcNow;
        existingOrder.UpdatedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Order), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}