using CRM.Admin.Data.ClientDTO;

namespace CRM.Admin.Requests.ClientRequests;

public interface IClientRequest
{
    Task<Guid> CreateAsync(ClientCreateDTO clientCreateDTO);
    Task<List<ClientDTO>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IClientDTO;
    Task<bool> UpdateAsync(ClientUpdateDTO clientUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}