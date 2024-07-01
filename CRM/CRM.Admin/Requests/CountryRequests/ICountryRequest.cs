using CRM.Admin.Data.CountryDto;

namespace CRM.Admin.Requests.CountryRequests;

public interface ICountryRequest
{
    Task<Guid> CreateAsync(CountryCreateDto countryCreateDTO);
    Task<List<CountryDto>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : ICountryDto;
    Task<CountryDto> GetByNameAsync(string name);
    Task<bool> UpdateAsync(CountryUpdateDto countryUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
}