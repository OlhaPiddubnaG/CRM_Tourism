using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Order;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderHandlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreateOrderHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        if (companyId != request.CompanyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to create this order.");
        }

        var order = _mapper.Map<Order>(request);
        var countryFrom = await _context.Countries.FindAsync(request.CountryFromId);
        var countryTo = await _context.Countries.FindAsync(request.CountryToId);

        if (countryFrom == null || countryTo == null)
        {
            throw new KeyNotFoundException("Country not found.");
        }

        order.CountryFrom = countryFrom;
        order.CountryTo = countryTo;

        order.CreatedAt = DateTime.UtcNow;
        order.CreatedUserId = _currentUser.GetUserId();
        order.OrderStatus = request.LatestStatus;

        var orderStatusHistory = new OrderStatusHistory
        {
            OrderId = order.Id,
            DateTime = DateTime.UtcNow,
            OrderStatus = request.LatestStatus,
            CreatedAt = DateTime.UtcNow,
            CreatedUserId = _currentUser.GetUserId()
        };

        order.OrderStatusHistory.Add(orderStatusHistory);
        _context.Orders.Add(order);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Order), ex);
        }

        return new CreatedResponse(order.Id);
    }
}