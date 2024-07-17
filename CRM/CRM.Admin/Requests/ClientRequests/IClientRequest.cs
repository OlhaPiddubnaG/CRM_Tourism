using CRM.Admin.Data;
using CRM.Admin.Data.ClientDto;

namespace CRM.Admin.Requests.ClientRequests;

public interface IClientRequest
{
    Task<Guid> CreateAsync(ClientCreateDto clientCreateDto);
    Task<ResultModel> CreateClientWithRelatedAsync(ClientCreateDto clientCreateDto);
    Task<List<ClientDto>> GetAllAsync();
    Task<PagedResponse<ClientDto>> GetPagedDataAsync(ClientRequestParameters parameters);
    Task<T> GetByIdAsync<T>(Guid id) where T : IClientDto;
    Task<bool> UpdateAsync(ClientUpdateDto clientUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}