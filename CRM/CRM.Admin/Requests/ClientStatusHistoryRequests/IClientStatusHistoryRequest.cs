using CRM.Admin.Data.ClientStatusHistoryDto;

namespace CRM.Admin.Requests.ClientStatusHistoryRequests;

public interface IClientStatusHistoryRequest
{
    Task<Guid> CreateAsync(ClientStatusHistoryCreateDto dto);
    Task<List<ClientStatusHistoryDto>> GetAllAsync(Guid clientId);
    Task<ClientStatusHistoryDto> GetByIdAsync(Guid id);
}