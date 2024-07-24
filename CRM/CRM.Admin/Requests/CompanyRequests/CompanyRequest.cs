using CRM.Admin.Data;
using CRM.Admin.Data.CompanyDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.CompanyRequests;

public class CompanyRequest : ICompanyRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<CompanyRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Company";

    public CompanyRequest(IHttpRequests httpRequests, ILogger<CompanyRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<List<CompanyDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<CompanyDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх компаній успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних компаній: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<CompanyUpdateDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<CompanyUpdateDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані компанії успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні даних компанії: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(CompanyUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for company with id: {dto.Id}");
                _snackbar.Add("Компанію успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for company with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні компанії: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for company with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні компанії: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Компанію успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні компанії: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні компанії: {ex.Message}", Severity.Error);
            throw;
        }
    }
}