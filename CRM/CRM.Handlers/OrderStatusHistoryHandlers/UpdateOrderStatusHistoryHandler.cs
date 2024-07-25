using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.OrderStatusHistory;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderStatusHistoryHandlers;

public class UpdateOrderStatusHistoryHandler : IRequestHandler<UpdateOrderStatusHistoryCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdateOrderStatusHistoryHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdateOrderStatusHistoryCommand request, CancellationToken cancellationToken)
    {
        var existingOrderStatusHistory = await _context.OrderStatusHistory
            .Include(o => o.Order)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingOrderStatusHistory == null)
        {
            throw new NotFoundException(typeof(OrderStatusHistory), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingOrderStatusHistory.Order.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update orderStatusHistory.");
        }
        
        existingOrderStatusHistory.DateTime = request.DateTime;
        existingOrderStatusHistory.OrderStatus = request.OrderStatus;
        existingOrderStatusHistory.UpdatedAt = DateTime.UtcNow;
        existingOrderStatusHistory.UpdatedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(OrderStatusHistory), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}