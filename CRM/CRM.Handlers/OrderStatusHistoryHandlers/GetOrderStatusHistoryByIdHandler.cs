using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.OrderStatusHistory;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderStatusHistoryHandlers;

public class GetOrderStatusHistoryByIdHandler : IRequestHandler<GetByIdRequest<OrderStatusHistoryResponse>,
    OrderStatusHistoryResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetOrderStatusHistoryByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<OrderStatusHistoryResponse> Handle(GetByIdRequest<OrderStatusHistoryResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var orderStatusHistory = await _context.OrderStatusHistory
            .FirstOrDefaultAsync(o => o.Id == request.Id &&
                                      o.Order.CompanyId == companyId &&
                                      !o.IsDeleted);

        if (orderStatusHistory == null)
        {
            throw new NotFoundException(typeof(OrderStatusHistory), request.Id);
        }

        var orderResponse = _mapper.Map<OrderStatusHistoryResponse>(orderStatusHistory);

        return orderResponse;
    }
}