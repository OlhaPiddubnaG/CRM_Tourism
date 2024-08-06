using CRM.Admin.Data.NumberOfPeopleDto;

namespace CRM.Admin.Requests.NumberOfPeopleRequests;

public interface INumberOfPeopleRequest
{
    Task<Guid> CreateAsync(NumberOfPeopleCreateDto dto);
    Task<List<NumberOfPeopleDto>> GetAllAsync();
    Task<NumberOfPeopleDto> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(NumberOfPeopleUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}