using CRM.Admin.Data.ClientStatusHistoryDto;

namespace CRM.Admin.Requests.ClientStatusHistoryRequests;

public interface IClientStatusHistoryRequest
{
    Task<Guid> CreateAsync(ClientStatusHistoryCreateDto clientStatusHistoryCreateDTO);
    Task<List<ClientStatusHistoryDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IClientStatusHistoryDto;
}