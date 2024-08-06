using CRM.Admin.Data;
using CRM.Admin.Data.StaysDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.StaysRequests;

public class StaysRequest : IStaysRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<StaysRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Stays";

    public StaysRequest(IHttpRequests httpRequests, ILogger<StaysRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(StaysCreateDto dto)
    {
        try
        {
            var createdStays = await _httpRequests.SendPostRequestAsync<StaysDto>(RequestUri, dto);
            _logger.LogInformation("CreateAsync method executed successfully");
            _snackbar.Add("Заклад проживання успішно створено", Severity.Success);

            return createdStays.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync method");
            _snackbar.Add($"Помилка при створенні закладу проживання: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<StaysDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<StaysDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх закладів проживання успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх закладів проживання: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<StaysDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<StaysDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані закладу проживання успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні закладу проживання через id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(StaysUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for stay with id: {dto.Id}");
                _snackbar.Add("Заклад проживання успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"UpdateAsync method failed for stay with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні закладу проживання: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for stay with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні закладу проживання: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Заклад проживання успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні закладу проживання: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні закладу проживання: {ex.Message}", Severity.Error);
            throw;
        }
    }
}