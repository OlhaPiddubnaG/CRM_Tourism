using CRM.Admin.Data;
using CRM.Admin.Data.ClientDto;
using MudBlazor;

namespace CRM.Admin.Requests.ClientRequests;

public interface IClientRequest
{
    Task<Guid> CreateAsync(ClientCreateDto clientCreateDto);
    Task<ResultModel> CreateClientWithRelatedAsync(ClientCreateDto clientCreateDto);
    Task<List<ClientDto>> GetAllAsync();
    Task<TableData<ClientDto>> GetFilteredAndSortedAsync(int page, int pageSize, string searchString, string sortLabel,
        SortDirection sortDirection);
    Task<T> GetByIdAsync<T>(Guid id) where T : IClientDto;
    Task<bool> UpdateAsync(ClientUpdateDto clientUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}