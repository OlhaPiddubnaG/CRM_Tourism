using CRM.Admin.Data.OrderDto;

namespace CRM.Admin.Requests.OrderRequests;

public interface IOrderRequest
{
    Task<Guid> CreateAsync(OrderCreateDto orderCreateDto);
    Task<List<OrderDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IOrderDto;
    Task<bool> UpdateAsync(OrderUpdateDto orderUpdateDto);
    Task<bool> DeleteAsync(Guid id);
}