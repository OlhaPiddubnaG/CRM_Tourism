using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Entities;
using CRM.Domain.Requests;
using CRM.Domain.Responses.Payment;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PaymentHandlers;

public class GetPaymentByIdHandler : IRequestHandler<GetByIdRequest<PaymentResponse>,
    PaymentResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public GetPaymentByIdHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<PaymentResponse> Handle(GetByIdRequest<PaymentResponse> request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.Id == request.Id &&
                                      p.Order.CompanyId == companyId &&
                                      !p.Order.IsDeleted &&
                                      !p.IsDeleted);

        if (payment == null)
        {
            throw new NotFoundException(typeof(Payment), request.Id);
        }

        var paymentResponse = _mapper.Map<PaymentResponse>(payment);

        return paymentResponse;
    }
}