using CRM.Admin.Data.OrderDto;

namespace CRM.Admin.Requests.OrderRequests;

public interface IOrderRequest
{
    Task<Guid> CreateAsync(OrderCreateDto dto);
    Task<List<OrderDto>> GetAllAsync();
    Task<OrderDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(OrderUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}