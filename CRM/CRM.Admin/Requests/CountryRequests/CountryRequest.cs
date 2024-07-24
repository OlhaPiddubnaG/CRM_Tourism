using CRM.Admin.Data;
using CRM.Admin.Data.CountryDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.CountryRequests;

public class CountryRequest : ICountryRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<CountryRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Country";

    public CountryRequest(IHttpRequests httpRequests, ILogger<CountryRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(CountryCreateDto dto)
    {
        try
        {
            var createdCountry = await _httpRequests.SendPostRequestAsync<CountryDto>(RequestUri, dto);
            _logger.LogInformation("Create country method executed successfully");
            _snackbar.Add("Країну успішно створено", Severity.Success);

            return createdCountry.Id;
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
            var result = await _httpRequests.SendGetRequestAsync<List<CountryDto>>(RequestUri);
            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх країн успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних країн: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<CountryDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<CountryDto>($"{RequestUri}/{id}");
            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані країни успішно завантажено", Severity.Success);

            return result;
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
            var result = await _httpRequests.SendGetRequestAsync<CountryDto>($"{RequestUri}/name/{name}");
            _logger.LogInformation($"GetByNameAsync method executed successfully for name: {name}");
            _snackbar.Add("Дані країни успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByNameAsync method for name: {name}");
            _snackbar.Add($"Помилка при завантаженні даних країни: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(CountryUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for country with id: {dto.Id}");
                _snackbar.Add("Країну успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for country with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні країни: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for country with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні країни: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendDeleteRequestAsync<ResultModel>($"{RequestUri}/{id}");

            if (result.Success)
            {
                _logger.LogInformation($"DeleteAsync method executed successfully for id: {id}");
                _snackbar.Add("Країну успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні країни: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні країни: {ex.Message}", Severity.Error);
            throw;
        }
    }
}