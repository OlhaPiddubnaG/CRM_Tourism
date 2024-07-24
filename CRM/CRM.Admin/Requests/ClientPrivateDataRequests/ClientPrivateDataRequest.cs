using CRM.Admin.Data;
using CRM.Admin.Data.ClientPrivateDataDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.ClientPrivateDataRequests;

public class ClientPrivateDataRequest : IClientPrivateDataRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<ClientPrivateDataRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/ClientPrivateData";

    public ClientPrivateDataRequest(IHttpRequests httpRequests, ILogger<ClientPrivateDataRequest> logger,
        ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(ClientPrivateDataCreateDto dto)
    {
        try
        {
            var createdClientPrivateData =
                await _httpRequests.SendPostRequestAsync<ClientPrivateDataDto>(RequestUri, dto);
            _logger.LogInformation("Create clientPrivateData method executed successfully");

            _snackbar.Add("Приватні дані клієнта успішно створено", Severity.Success);
            return createdClientPrivateData.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create clientPrivateData method");
            _snackbar.Add($"Помилка при створенні приватних даних клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<ClientPrivateDataDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<ClientPrivateDataDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх клієнтів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних клієнтів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<ClientPrivateDataDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<ClientPrivateDataDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані клієнта успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні даних клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<ClientPrivateDataDto> GetByClientIdAsync(Guid clientId)
    {
        try
        {
            var result =
                await _httpRequests.SendGetRequestAsync<ClientPrivateDataDto>($"{RequestUri}/by-client-id/{clientId}");

            _logger.LogInformation($"GetByClientIdAsync method executed successfully for clientId: {clientId}");
            _snackbar.Add("Дані клієнта успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByClientIdAsync method for clientId: {clientId}");
            _snackbar.Add($"Помилка при завантаженні даних клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(ClientPrivateDataUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for clientPrivateData with id: {dto.Id}");
                _snackbar.Add("Дані клієнта успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for clientPrivateData with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні даних клієнта: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for clientPrivateData with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні даних клієнта: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Дані клієнта успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні даних клієнта: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні даних клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }
}