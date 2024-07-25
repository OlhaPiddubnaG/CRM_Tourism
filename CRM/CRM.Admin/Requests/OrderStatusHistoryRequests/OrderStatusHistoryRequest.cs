using CRM.Admin.Data;
using CRM.Admin.Data.OrderDto;
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

    public async Task<Guid> CreateAsync(OrderStatusHistoryCreateDto dto)
    {
        try
        {
            var createdOrder = await _httpRequests.SendPostRequestAsync<OrderDto>(RequestUri, dto);
            _logger.LogInformation("Create orderStatusHistory method executed successfully");
            _snackbar.Add("Історія статусу замовлення успішно створено", Severity.Success);

            return createdOrder.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create orderStatusHistory method");
            _snackbar.Add($"Помилка при створенні історії статусу замовлення: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<OrderStatusHistoryDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<OrderStatusHistoryDto>>(RequestUri);

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

    public async Task<bool> UpdateAsync(OrderStatusHistoryUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation(
                    $"UpdateAsync method executed successfully for orderStatusHistory with id: {dto.Id}");
                _snackbar.Add("Історія статусу замовлення успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for orderStatusHistory with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні історії статусу  замовлення: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for orderStatusHistory with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні історії статусу замовлення: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Історія статусу замовлення успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні історії статусу замовлення: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні історії статусу замовлення: {ex.Message}", Severity.Error);
            throw;
        }
    }
}