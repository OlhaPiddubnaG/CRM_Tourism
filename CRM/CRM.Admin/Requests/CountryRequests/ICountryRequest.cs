using CRM.Admin.Data.CountryDTO;

namespace CRM.Admin.Requests.CountryRequests;

public interface ICountryRequest
{
    Task<Guid> CreateAsync(CountryCreateDTO countryCreateDTO);
    Task<List<CountryDTO>> GetAllAsync();
    Task<T> GetByIdAsync<T>(Guid id) where T : ICountryDTO;
    Task<bool> UpdateAsync(CountryUpdateDTO countryUpdateDTO);
    Task<bool> DeleteAsync(Guid id);
    Task<CountryDTO> GetByNameAsync(string name);
}