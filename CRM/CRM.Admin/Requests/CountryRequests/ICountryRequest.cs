using CRM.Admin.Data.CountryDto;

namespace CRM.Admin.Requests.CountryRequests;

public interface ICountryRequest
{
    Task<Guid> CreateAsync(CountryCreateDto dto);
    Task<List<CountryDto>> GetAllAsync();
    Task<CountryDto> GetByIdAsync(Guid id);
    Task<CountryDto> GetByNameAsync(string name);
    Task<bool> UpdateAsync(CountryUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}