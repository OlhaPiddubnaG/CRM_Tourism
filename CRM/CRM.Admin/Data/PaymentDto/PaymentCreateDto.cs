using CRM.Domain.Enums;

namespace CRM.Admin.Data.PaymentDto;

public class PaymentCreateDto
{
    public Guid OrderId { get; set; } 
    public DateTime? DateTime { get; set; } = System.DateTime.UtcNow;
    public decimal Amount { get; set; }
    public TypeOfPayment TypeOfPayment { get; set; }
}