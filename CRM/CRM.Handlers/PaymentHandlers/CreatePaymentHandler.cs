using AutoMapper;
using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Payment;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PaymentHandlers;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CreatedResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;

    public CreatePaymentHandler(AppDbContext context, IMapper mapper, ICurrentUser currentUser)
    {
        _context = context;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<CreatedResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var companyId = _currentUser.GetCompanyId();

        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId &&
                                      o.CompanyId == companyId &&
                                      !o.IsDeleted, cancellationToken);

        if (order == null)
        {
            throw new InvalidOperationException(
                "Order not found or user is not authorized to create payment for an order from a different company.");
        }

        var payment = _mapper.Map<Payment>(request);

        payment.CreatedAt = DateTime.UtcNow;
        payment.CreatedUserId = _currentUser.GetUserId();
        _context.Payments.Add(payment);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Payment), ex);
        }

        return new CreatedResponse(payment.Id);
    }
}