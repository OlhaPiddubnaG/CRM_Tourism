using AutoMapper;
using CRM.DataAccess;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Payment;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PaymentHandlers;

public class GetAllPaymentsHandler : IRequestHandler<GetAllRequest<PaymentResponse>,
    List<PaymentResponse>>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetAllPaymentsHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<PaymentResponse>> Handle(GetAllRequest<PaymentResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var payments = await _context.Payments
            .Where(p => p.Order.CompanyId == companyId &&
                        !p.Order.IsDeleted &&
                        !p.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!payments.Any())
        {
            return new List<PaymentResponse>();
        }

        var paymentResponses = _mapper.Map<List<PaymentResponse>>(payments);

        return paymentResponses;
    }
}