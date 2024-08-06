using CRM.Admin.Data;
using CRM.Admin.Data.PaymentDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.PaymentRequests;

public class PaymentRequest : IPaymentRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<PaymentRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Payment";

    public PaymentRequest(IHttpRequests httpRequests, ILogger<PaymentRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(PaymentCreateDto dto)
    {
        try
        {
            var createdPayment = await _httpRequests.SendPostRequestAsync<PaymentDto>(RequestUri, dto);
            _logger.LogInformation("CreateAsync method executed successfully");
            _snackbar.Add("Платіж успішно створено", Severity.Success);

            return createdPayment.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync method");
            _snackbar.Add($"Помилка при створенні платежу: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<PaymentDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<PaymentDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх платежів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх платежів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<PaymentDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<PaymentDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані платежу успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні платежу за id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(PaymentUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for payment with id: {dto.Id}");
                _snackbar.Add("Платіж успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for payment with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні платежу: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for payment with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні платежу: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Платіж успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні платежу: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні платежу: {ex.Message}", Severity.Error);
            throw;
        }
    }
}