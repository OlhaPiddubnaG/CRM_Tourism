using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.OrderStatusHistory;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderStatusHistoryHandlers;

public class CreateOrderStatusHistoryHandler : IRequestHandler<CreateOrderStatusHistoryCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateOrderStatusHistoryHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateOrderStatusHistoryCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var order = await _context.Orders
            .FirstOrDefaultAsync(c => c.Id == request.OrderId &&
                                      c.CompanyId == companyId &&
                                      !c.IsDeleted, cancellationToken);

        if (order == null)
        {
            throw new UnauthorizedAccessException(
                "User is not authorized to create status history for a order from a different company.");
        }

        var orderStatusHistory = _mapper.Map<OrderStatusHistory>(request);

        orderStatusHistory.CreatedAt = DateTime.UtcNow;
        orderStatusHistory.CreatedUserId = _currentUser.GetUserId();

        _context.OrderStatusHistory.Add(orderStatusHistory);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(OrderStatusHistory), ex);
        }

        return new CreatedResponse(orderStatusHistory.Id);
    }
}