using CRM.Admin.Data.PaymentDto;

namespace CRM.Admin.Requests.PaymentRequests;

public interface IPaymentRequest
{
    Task<Guid> CreateAsync(PaymentCreateDto dto);
    Task<List<PaymentDto>> GetAllAsync();
    Task<PaymentDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(PaymentUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}