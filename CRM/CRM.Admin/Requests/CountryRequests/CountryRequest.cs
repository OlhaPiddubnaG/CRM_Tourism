using System.Net;
using CRM.Admin.Data.CountryDto;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.CountryRequests;

public class CountryRequest : ICountryRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<CountryRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Country";

    public CountryRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<CountryRequest> logger, ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(CountryCreateDto countryCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, countryCreateDto);
            _logger.LogInformation("Create country method executed successfully");

            var createdClient = await response.Content.ReadFromJsonAsync<CountryDto>();
            _snackbar.Add("Країну успішно створено", Severity.Success);
            return createdClient.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create country method");
            _snackbar.Add($"Помилка при створенні країни: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<CountryDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх країн успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<List<CountryDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних країн: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : ICountryDto
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані країни успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<T>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні даних країни: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<CountryDto> GetByNameAsync(string name)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/name/{name}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _snackbar.Add($"Країна з ім'ям {name} не знайдена", Severity.Warning);
                return null;
            }

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"GetByNameAsync method executed successfully for name: {name}");
            _snackbar.Add("Дані країни успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<CountryDto>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByNameAsync method for name: {name}");
            _snackbar.Add($"Помилка при завантаженні даних країни: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(CountryUpdateDto countryUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, countryUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                $"UpdateAsync method executed successfully for country with id: {countryUpdateDto.Id}");
            _snackbar.Add("Країну успішно оновлено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for country with id: {countryUpdateDto.Id}");
            _snackbar.Add($"Помилка при оновленні країни: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendDeleteRequestAsync($"{RequestUri}/{id}");
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"DeleteAsync method executed successfully for id: {id}");
            _snackbar.Add("Країну успішно видалено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні країни: {ex.Message}", Severity.Error);
            throw;
        }
    }
}