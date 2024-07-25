using CRM.Admin.Data.OrderStatusHistoryDto;

namespace CRM.Admin.Requests.OrderStatusHistoryRequests;

public interface IOrderStatusHistoryRequest
{
    Task<Guid> CreateAsync(OrderStatusHistoryCreateDto dto);
    Task<List<OrderStatusHistoryDto>> GetAllAsync();
    Task<OrderStatusHistoryUpdateDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(OrderStatusHistoryUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}