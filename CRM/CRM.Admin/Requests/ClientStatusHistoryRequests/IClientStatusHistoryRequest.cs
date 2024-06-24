using CRM.Admin.Data.ClientStatusHistoryDTO;

namespace CRM.Admin.Requests.ClientStatusHistoryRequests;

public interface IClientStatusHistoryRequest
{
    Task<Guid> CreateAsync(ClientStatusHistoryCreateDTO clientStatusHistoryCreateDTO);
    Task<List<ClientStatusHistoryDTO>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IClientStatusHistoryDTO;
    Task<bool> UpdateAsync(ClientStatusHistoryUpdateDTO clientStatusHistoryUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}