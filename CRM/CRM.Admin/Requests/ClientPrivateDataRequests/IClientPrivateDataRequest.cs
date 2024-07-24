using CRM.Admin.Data.ClientPrivateDataDto;

namespace CRM.Admin.Requests.ClientPrivateDataRequests;

public interface IClientPrivateDataRequest
{
    Task<Guid> CreateAsync(ClientPrivateDataCreateDto dto);
    Task<List<ClientPrivateDataDto>> GetAllAsync();
    Task<ClientPrivateDataDto> GetByIdAsync(Guid id);
    Task<ClientPrivateDataDto> GetByClientIdAsync(Guid clientId);
    Task<bool> UpdateAsync(ClientPrivateDataUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}