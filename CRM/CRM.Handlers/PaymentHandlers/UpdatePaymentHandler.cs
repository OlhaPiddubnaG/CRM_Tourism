using CRM.Core.Exceptions;
using CRM.DataAccess;
using CRM.Domain.Commands.Payment;
using CRM.Domain.Entities;
using CRM.Domain.Responses;
using CRM.Handlers.Services.CurrentUser;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRM.Handlers.PaymentHandlers;

public class UpdatePaymentHandler : IRequestHandler<UpdatePaymentCommand, ResultBaseResponse>
{
    private readonly AppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public UpdatePaymentHandler(AppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<ResultBaseResponse> Handle(UpdatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var existingPayment = await _context.Payments
            .Include(o => o.Order)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (existingPayment == null)
        {
            throw new NotFoundException(typeof(Payment), request.Id);
        }

        var companyId = _currentUser.GetCompanyId();

        if (existingPayment.Order.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update payment.");
        }

        existingPayment.Amount = request.Amount;
        existingPayment.TypeOfPayment = request.TypeOfPayment;
        existingPayment.UpdatedAt = DateTime.UtcNow;
        existingPayment.UpdatedUserId = _currentUser.GetUserId();

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new SaveDatabaseException(typeof(Payment), ex);
        }

        return new ResultBaseResponse
        {
            Success = true,
            Message = "Successfully updated."
        };
    }
}