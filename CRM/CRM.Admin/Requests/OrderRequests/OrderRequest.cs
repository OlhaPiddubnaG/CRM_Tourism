using CRM.Admin.Data;
using CRM.Admin.Data.ClientDto;
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

    public async Task<ResultModel> CreateOrderWithRelatedAsync(OrderCreateDto dto)
    {
        try
        {
            var orderResponse =
                await _httpRequests.SendPostRequestAsync<ResultModel>($"{RequestUri}/withRelated", dto);

            if (orderResponse != null && orderResponse.Success)
            {
                _snackbar.Add("Замовлення та пов’язані дані успішно створено", Severity.Success);
                return new ResultModel
                {
                    Success = true,
                    Message = "Create successfully."
                };
            }
            else
            {
                _snackbar.Add("Сталася помилка під час обробки вашого запиту", Severity.Error);
                return new ResultModel
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateOrderWithRelatedAsync method");
            _snackbar.Add($"Помилка при створенні замовлення з пов’язаними даними: {ex.Message}", Severity.Error);
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
    
    public async Task<PagedResponse<OrderDto>> GetPagedDataAsync(OrderRequestParameters parameters)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "searchString", parameters.SearchString ?? string.Empty },
            { "sortLabel", parameters.SortLabel ?? string.Empty },
            { "sortDirection", parameters.SortDirection.ToString() },
            { "page", parameters.PageIndex.ToString() },
            { "pageSize", parameters.PageSize.ToString() }
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var pagedResponse =
            await _httpRequests.SendPostRequestAsync<PagedResponse<OrderDto>>($"{RequestUri}/paged?{queryString}",
                parameters);

        return pagedResponse;
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