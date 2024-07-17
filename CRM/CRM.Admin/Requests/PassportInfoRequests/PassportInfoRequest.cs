using System.Net;
using CRM.Admin.Data.PassportInfoDto;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.PassportInfoRequests;

public class PassportInfoRequest : IPassportInfoRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<PassportInfoRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/PassportInfo";

    public PassportInfoRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<PassportInfoRequest> logger,
        ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(PassportInfoCreateDto passportInfoCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, passportInfoCreateDto);
            _logger.LogInformation("Create passport info method executed successfully");

            var createdPassportInfo = await response.Content.ReadFromJsonAsync<PassportInfoDto>();
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
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх паспортів успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<List<PassportInfoDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних паспортів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IPassportInfoDto
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані паспорту успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<T>(content);
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
            var response =
                await _httpCrmApiRequests.SendGetRequestAsync(
                    $"{RequestUri}/by-client-private-data-id/{clientPrivateDataId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation(
                $"GetByClientPrivateDataIdAsync method executed successfully for ClientPrivateDataId: {clientPrivateDataId}");
            _snackbar.Add("Дані паспортів клієнта успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<List<PassportInfoDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Error in GetByClientPrivateDataIdAsync method for ClientPrivateDataId: {clientPrivateDataId}");
            _snackbar.Add($"Помилка при завантаженні даних паспортів клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(PassportInfoUpdateDto passportInfoUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, passportInfoUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                $"UpdateAsync method executed successfully for user with id: {passportInfoUpdateDto.Id}");
            _snackbar.Add("Паспортні дані успішно оновлено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {passportInfoUpdateDto.Id}");
            _snackbar.Add($"Помилка при оновленні паспортних даних: {ex.Message}", Severity.Error);
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
            _snackbar.Add("Паспортні дані успішно видалено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні паспортних даних: {ex.Message}", Severity.Error);
            throw;
        }
    }
}