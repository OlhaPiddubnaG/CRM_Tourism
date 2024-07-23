using System.Net;
using CRM.Admin.Data.TouroperatorDto;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.TouroperatorRequests;

public class TouroperatorRequest : ITouroperatorRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<TouroperatorRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Touroperator";

    public TouroperatorRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<TouroperatorRequest> logger,
        ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(TouroperatorCreateDto touroperatorCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, touroperatorCreateDto);
            _logger.LogInformation("Create touroperator method executed successfully");

            var createdClient = await response.Content.ReadFromJsonAsync<TouroperatorDto>();
            _snackbar.Add("Туроператора успішно створено", Severity.Success);
            return createdClient.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
            _snackbar.Add($"Помилка при створенні туроператора: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<TouroperatorDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<TouroperatorDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх туроператорів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : ITouroperatorDto
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            return JsonConvert.DeserializeObject<T>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні туроператора з id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(TouroperatorUpdateDto touroperatorUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, touroperatorUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                $"UpdateAsync method executed successfully for user with id: {touroperatorUpdateDto.Id}");
            _snackbar.Add("Туроператора успішно оновлено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for touroperator with id: {touroperatorUpdateDto.Id}");
            _snackbar.Add($"Помилка при оновленні туроператора: {ex.Message}", Severity.Error);
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
            _snackbar.Add("Туроператора успішно видалено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні туроператора: {ex.Message}", Severity.Error);
            throw;
        }
    }
}