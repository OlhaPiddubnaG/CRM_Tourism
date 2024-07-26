using CRM.Admin.Data.ClientStatusHistoryDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.ClientStatusHistoryRequests;

public class ClientStatusHistoryRequest : IClientStatusHistoryRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<ClientStatusHistoryRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/ClientStatusHistory";

    public ClientStatusHistoryRequest(IHttpRequests httpRequests,
        ILogger<ClientStatusHistoryRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(ClientStatusHistoryCreateDto dto)
    {
        try
        {
            var createdClientStatusHistory =
                await _httpRequests.SendPostRequestAsync<ClientStatusHistoryDto>(RequestUri, dto);
            _logger.LogInformation("Create ClientStatusHistory method executed successfully");

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

    public async Task<List<ClientStatusHistoryDto>> GetAllAsync(Guid clientId)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<ClientStatusHistoryDto>>($"{RequestUri}/client/{clientId}");

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх історій статусів клієнтів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні даних історій статусів клієнтів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<ClientStatusHistoryDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<ClientStatusHistoryDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані історії статусу клієнта успішно завантажено", Severity.Success);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні даних історії статусу клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }
}