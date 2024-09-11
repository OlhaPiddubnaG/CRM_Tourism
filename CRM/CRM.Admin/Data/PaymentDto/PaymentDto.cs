using CRM.Domain.Enums;

namespace CRM.Admin.Data.PaymentDto;

public class PaymentDto
{
    public Guid Id { get; set; } 
    public Guid OrderId { get; set; } 
    public DateTime? DateTime { get; set; } 
    public decimal Amount { get; set; }
    public TypeOfPayment TypeOfPayment { get; set; }
}