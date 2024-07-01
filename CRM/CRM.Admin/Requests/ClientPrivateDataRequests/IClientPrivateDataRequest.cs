using CRM.Admin.Data.ClientPrivateDataDto;

namespace CRM.Admin.Requests.ClientPrivateDataRequests;

public interface IClientPrivateDataRequest
{
    Task<Guid> CreateAsync(ClientPrivateDataCreateDto clientPrivateDataCreateDTO);
    Task<List<ClientPrivateDataDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : IClientPrivateDataDto;
    Task<T> GetByClientIdAsync<T>(Guid clientId) where T : IClientPrivateDataDto;
    Task<bool> UpdateAsync(ClientPrivateDataUpdateDto clientPrivateDataUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}