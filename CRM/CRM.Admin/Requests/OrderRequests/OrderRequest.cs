using CRM.Admin.Data;
using CRM.Admin.Data.OrderDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.OrderRequests;

public class OrderRequest : IOrderRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<OrderRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Order";

    public OrderRequest(IHttpRequests httpRequests, ILogger<OrderRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(OrderCreateDto dto)
    {
        try
        {
            var createdOrder = await _httpRequests.SendPostRequestAsync<OrderDto>(RequestUri, dto);
            _logger.LogInformation("Create order method executed successfully");
            _snackbar.Add("Замовлення успішно створено", Severity.Success);
            
            return createdOrder.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create order method");
            _snackbar.Add($"Помилка при створенні замовлення: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<OrderDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх замовлень успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх замовлень: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<OrderDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<OrderDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані замовлення успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні замовлення через id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(OrderUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for order with id: {dto.Id}");
                _snackbar.Add("Замовлення успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"UpdateAsync method failed for order with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні замовлення: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for order with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні замовлення: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Замовлення успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні замовлення: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні замовлення: {ex.Message}", Severity.Error);
            throw;
        }
    }
}