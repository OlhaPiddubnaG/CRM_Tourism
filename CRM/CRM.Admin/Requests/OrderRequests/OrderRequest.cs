using System.Net;
using CRM.Admin.Data.OrderDto;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.OrderRequests;

public class OrderRequest : IOrderRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<OrderRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Order";

    public OrderRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<OrderRequest> logger, ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(OrderCreateDto orderCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, orderCreateDto);
            _logger.LogInformation("Create order method executed successfully");

            var createdClient = await response.Content.ReadFromJsonAsync<OrderDto>();
            _snackbar.Add("Замовлення успішно створено", Severity.Success);
            return createdClient.Id;
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
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<OrderDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх замовлень: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IOrderDto
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
            _snackbar.Add($"Помилка при завантаженні замовлення через id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(OrderUpdateDto orderUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, orderUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"UpdateAsync method executed successfully for order with id: {orderUpdateDto.Id}");
            _snackbar.Add("Замовлення успішно оновлено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for order with id: {orderUpdateDto.Id}");
            _snackbar.Add($"Помилка при оновленні замовлення: {ex.Message}", Severity.Error);
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
            _snackbar.Add("Замовлення успішно видалено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні замовлення: {ex.Message}", Severity.Error);
            throw;
        }
    }
}