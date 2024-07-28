using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Order;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderHandlers;

public class GetAllOrdersHandler : IRequestHandler<GetAllRequest<OrderResponse>, List<OrderResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllOrdersHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<OrderResponse>> Handle(GetAllRequest<OrderResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var orders = await _context.Orders
            .Include(o => o.NumberOfPeople)
            .Include(o => o.CountryFrom)
            .Include(o => o.CountryTo)
            .Include(c => c.OrderStatusHistory)
            .Where(o => o.CompanyId == companyId &&
                        !o.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
        {
            return new List<OrderResponse>();
        }

        var orderResponses = _mapper.Map<List<OrderResponse>>(orders);

        foreach (var orderResponse in orderResponses)
        {
            var order = orders.FirstOrDefault(c => c.Id == orderResponse.Id);
            if (order != null)
            {
                orderResponse.LatestStatus = order.OrderStatus;
            }
        }

        return orderResponses;
    }
}