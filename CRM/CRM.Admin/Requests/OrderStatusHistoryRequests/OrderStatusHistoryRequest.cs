using CRM.Admin.Data.OrderStatusHistoryDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.OrderStatusHistoryRequests;

public class OrderStatusHistoryRequest : IOrderStatusHistoryRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<OrderStatusHistoryRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/OrderStatusHistory";

    public OrderStatusHistoryRequest(IHttpRequests httpRequests, ILogger<OrderStatusHistoryRequest> logger,
        ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<List<OrderStatusHistoryDto>> GetAllAsync(Guid orderId)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<OrderStatusHistoryDto>>($"{RequestUri}/order/{orderId}");

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх історій статусу замовлень успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх історій статусу замовлень: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<OrderStatusHistoryUpdateDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<OrderStatusHistoryUpdateDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані історії статусу замовлення успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні історії статусу замовлення через id: {id}, {ex.Message}",
                Severity.Error);
            throw;
        }
    }
}