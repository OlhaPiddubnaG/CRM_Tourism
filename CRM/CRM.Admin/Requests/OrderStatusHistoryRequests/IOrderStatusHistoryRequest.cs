using CRM.Admin.Data.OrderStatusHistoryDto;

namespace CRM.Admin.Requests.OrderStatusHistoryRequests;

public interface IOrderStatusHistoryRequest
{
    Task<List<OrderStatusHistoryDto>> GetAllAsync(Guid orderId);
    Task<OrderStatusHistoryUpdateDto> GetByIdAsync(Guid id);
}