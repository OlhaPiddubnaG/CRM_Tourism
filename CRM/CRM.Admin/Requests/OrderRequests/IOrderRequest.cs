using CRM.Admin.Data;
using CRM.Admin.Data.ClientDto;
using CRM.Admin.Data.OrderDto;

namespace CRM.Admin.Requests.OrderRequests;

public interface IOrderRequest
{
    Task<Guid> CreateAsync(OrderCreateDto dto);
    Task<ResultModel> CreateOrderWithRelatedAsync(OrderCreateDto dto);
    Task<List<OrderDto>> GetAllAsync();
    Task<PagedResponse<OrderDto>> GetPagedDataAsync(OrderRequestParameters parameters);
    Task<PagedResponse<OrderDto>> GetPagedDataByClientIdAsync(FilteredOrdersRequestParameters parameters);
    Task<OrderDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(OrderUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}