using CRM.Domain.Enums;

namespace CRM.Domain.Responses.Payment;

public class PaymentResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Amount { get; set; }
    public TypeOfPayment TypeOfPayment { get; set; }
    public bool IsDeleted { get; set; }
}