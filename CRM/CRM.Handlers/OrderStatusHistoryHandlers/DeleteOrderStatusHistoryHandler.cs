using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderStatusHistoryHandlers;

public class DeleteOrderStatusHistoryHandler : IRequestHandler<DeleteCommand<OrderStatusHistory>, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteOrderStatusHistoryHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(DeleteCommand<OrderStatusHistory> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var orderStatusHistory = await _context.OrderStatusHistory
            .Include(o => o.Order)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (orderStatusHistory == null)
        {
            throw new NotFoundException(typeof(OrderStatusHistory), request.Id);
        }

        if (companyId != orderStatusHistory.Order.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this orderStatusHistory.");
        }

        if (orderStatusHistory.IsDeleted)
        {
            throw new InvalidOperationException($"OrderStatusHistory with ID {request.Id} is already deleted.");
        }

        orderStatusHistory.IsDeleted = true;
        orderStatusHistory.DeletedAt = DateTime.UtcNow;
        orderStatusHistory.DeletedUserId = _currentUser.GetUserId();

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
            Message = "Successfully deleted."
        };
    }
}