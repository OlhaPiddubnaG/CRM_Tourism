using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands;
using CRM.Domain.Entities;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderHandlers;

public class DeleteOrderHandler : IRequestHandler<DeleteCommand<Order>, Unit>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public DeleteOrderHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteCommand<Order> request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var order = await _context.Orders.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (order == null)
        {
            throw new NotFoundException(typeof(Order), request.Id);
        }

        if (companyId != order.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to delete this order.");
        }

        if (order.IsDeleted)
        {
            throw new InvalidOperationException($"Order with ID {request.Id} is already deleted.");
        }

        order.IsDeleted = true;
        order.DeletedAt = DateTime.UtcNow;
        order.DeletedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Order), ex);
        }

        return Unit.Value;
    }
}