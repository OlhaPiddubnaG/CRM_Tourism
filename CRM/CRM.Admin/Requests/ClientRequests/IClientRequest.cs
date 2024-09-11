using CRM.Admin.Data;
using CRM.Admin.Data.ClientDto;

namespace CRM.Admin.Requests.ClientRequests;

public interface IClientRequest
{
    Task<Guid> CreateAsync(ClientCreateDto dto);
    Task<List<ClientDto>> GetFiltredDataAsync(string searchString);
    Task<ResultModel> CreateClientWithRelatedAsync(ClientCreateDto dto);
    Task<List<ClientDto>> GetAllAsync();
    Task<PagedResponse<ClientDto>> GetPagedDataAsync(ClientRequestParameters parameters);
    Task<ClientUpdateDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(ClientUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}