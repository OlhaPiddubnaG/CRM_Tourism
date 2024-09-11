using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Payment;

public class CreatePaymentCommand : IRequest<CreatedResponse>
{
    public Guid OrderId { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }
    public TypeOfPayment TypeOfPayment { get; set; }
}