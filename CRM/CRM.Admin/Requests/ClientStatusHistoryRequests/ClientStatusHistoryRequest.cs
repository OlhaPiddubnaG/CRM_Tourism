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
}