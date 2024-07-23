using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Order;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.OrderHandlers;

public class GetOrderByIdHandler : IRequestHandler<GetByIdRequest<OrderResponse>, OrderResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetOrderByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<OrderResponse> Handle(GetByIdRequest<OrderResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var order = await _context.Orders
            .Include(o => o.NumberOfPeople)
            .Include(o => o.CountryFrom)
            .Include(o => o.CountryTo)
            .FirstOrDefaultAsync(o => o.Id == request.Id &&
                                      o.CompanyId == companyId &&
                                      !o.IsDeleted);

        if (order == null)
        {
            throw new UnauthorizedAccessException("User is not authorized to access order.");
        }

        var orderResponse = _mapper.Map<OrderResponse>(order);

        return orderResponse;
    }
}