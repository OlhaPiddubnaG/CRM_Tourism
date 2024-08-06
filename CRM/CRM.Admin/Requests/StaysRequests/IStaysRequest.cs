using CRM.Admin.Data.StaysDto;

namespace CRM.Admin.Requests.StaysRequests;

public interface IStaysRequest
{
    Task<Guid> CreateAsync(StaysCreateDto dto);
    Task<List<StaysDto>> GetAllAsync();
    Task<StaysDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(StaysUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}