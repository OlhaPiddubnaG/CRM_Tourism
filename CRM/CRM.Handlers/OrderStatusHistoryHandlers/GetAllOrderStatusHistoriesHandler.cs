using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.OrderStatusHistory;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderStatusHistoryHandlers;

public class GetAllOrderStatusHistoriesHandler : IRequestHandler<GetAllRequest<OrderStatusHistoryResponse>,
    List<OrderStatusHistoryResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllOrderStatusHistoriesHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<OrderStatusHistoryResponse>> Handle(GetAllRequest<OrderStatusHistoryResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var orderStatusHistories = await _context.OrderStatusHistory
            .Include(o => o.Order)
            .Where(o => o.Order.CompanyId == companyId &&
                        !o.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!orderStatusHistories.Any())
        {
            return new List<OrderStatusHistoryResponse>();
        }

        var orderStatusHistoryResponses = _mapper.Map<List<OrderStatusHistoryResponse>>(orderStatusHistories);

        return orderStatusHistoryResponses;
    }
}