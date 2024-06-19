using CRM.Admin.Data.ClientPrivateDataDTO;

namespace CRM.Admin.Requests.ClientPrivateDataRequests;

public interface IClientPrivateDataRequest
{
    Task<Guid> CreateAsync(ClientPrivateDataCreateDTO clientPrivateDataCreateDTO);
    Task<List<ClientPrivateDataDTO>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IClientPrivateDataDTO;
    Task<bool> UpdateAsync(ClientPrivateDataUpdateDTO clientPrivateDataUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}