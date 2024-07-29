using CRM.Domain.Enums;
using CRM.Domain.Responses;
using MediatR;

namespace CRM.Domain.Commands.Payment;

public class UpdatePaymentCommand : IRequest<ResultBaseResponse>
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Amount { get; set; }
    public TypeOfPayment TypeOfPayment { get; set; }
}