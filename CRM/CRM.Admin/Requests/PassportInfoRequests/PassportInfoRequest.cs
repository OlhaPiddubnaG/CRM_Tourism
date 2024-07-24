using CRM.Admin.Data;
using CRM.Admin.Data.PassportInfoDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.PassportInfoRequests;

public class PassportInfoRequest : IPassportInfoRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<PassportInfoRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/PassportInfo";

    public PassportInfoRequest(IHttpRequests httpRequests, ILogger<PassportInfoRequest> logger,
        ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(PassportInfoCreateDto dto)
    {
        try
        {
            var createdPassportInfo = await _httpRequests.SendPostRequestAsync<PassportInfoDto>(RequestUri, dto);
            _logger.LogInformation("Create passport info method executed successfully");

            _snackbar.Add("Паспортні дані успішно створено", Severity.Success);
            return createdPassportInfo.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create passport info method");
            _snackbar.Add($"Помилка при створенні паспортних даних: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<PassportInfoDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<PassportInfoDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх паспортів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних паспортів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<PassportInfoUpdateDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<PassportInfoUpdateDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані паспорту успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні даних паспорту: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<PassportInfoDto>> GetByClientPrivateDataIdAsync(Guid clientPrivateDataId)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<PassportInfoDto>>(
                $"{RequestUri}/by-client-private-data-id/{clientPrivateDataId}");

            _logger.LogInformation(
                $"GetByClientPrivateDataIdAsync method executed successfully for ClientPrivateDataId: {clientPrivateDataId}");
            _snackbar.Add("Дані паспортів клієнта успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Error in GetByClientPrivateDataIdAsync method for ClientPrivateDataId: {clientPrivateDataId}");
            _snackbar.Add($"Помилка при завантаженні даних паспортів клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(PassportInfoUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for passport info with id: {dto.Id}");
                _snackbar.Add("Паспортні дані успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"UpdateAsync method failed for passport info with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні паспортних даних: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for passport info with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні паспортних даних: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Паспортні дані успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні паспортних даних: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні паспортних даних: {ex.Message}", Severity.Error);
            throw;
        }
    }
}