using CRM.Admin.Data.ClientStatusHistoryDto;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.ClientStatusHistoryRequests;

public class ClientStatusHistoryRequest : IClientStatusHistoryRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<ClientStatusHistoryRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/ClientStatusHistory";

    public ClientStatusHistoryRequest(IHttpCrmApiRequests httpCrmApiRequests,
        ILogger<ClientStatusHistoryRequest> logger, ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(ClientStatusHistoryCreateDto clientStatusHistoryCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, clientStatusHistoryCreateDto);
            _logger.LogInformation("Create ClientStatusHistory method executed successfully");

            var createdClientStatusHistory = await response.Content.ReadFromJsonAsync<ClientStatusHistoryDto>();
            _snackbar.Add("Історія статусу клієнта успішно створена", Severity.Success);
            return createdClientStatusHistory.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
            _snackbar.Add($"Помилка при створенні історії статусу клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<ClientStatusHistoryDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх історій статусів клієнтів успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<List<ClientStatusHistoryDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних історій статусів клієнтів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IClientStatusHistoryDto
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані історії статусу клієнта успішно завантажено", Severity.Success);
            return JsonConvert.DeserializeObject<T>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні даних історії статусу клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }
}