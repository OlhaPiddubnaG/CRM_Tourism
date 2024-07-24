using CRM.Admin.Data;
using CRM.Admin.Data.TouroperatorDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.TouroperatorRequests;

public class TouroperatorRequest : ITouroperatorRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<TouroperatorRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Touroperator";

    public TouroperatorRequest(IHttpRequests httpRequests, ILogger<TouroperatorRequest> logger,
        ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(TouroperatorCreateDto dto)
    {
        try
        {
            var createdTouroperator = await _httpRequests.SendPostRequestAsync<TouroperatorDto>(RequestUri, dto);
            _logger.LogInformation("Create touroperator method executed successfully");
            _snackbar.Add("Туроператора успішно створено", Severity.Success);

            return createdTouroperator.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create touroperator method");
            _snackbar.Add($"Помилка при створенні туроператора: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<TouroperatorDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<TouroperatorDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх туроператорів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх туроператорів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<TouroperatorDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<TouroperatorDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані туроператора успішно завантажено", Severity.Success);

            return result;
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
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, touroperatorUpdateDto);

            if (result.Success)
            {
                _logger.LogInformation(
                    $"UpdateAsync method executed successfully for touroperator with id: {touroperatorUpdateDto.Id}");
                _snackbar.Add("Туроператора успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for touroperator with id: {touroperatorUpdateDto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні туроператора: {result.Message}", Severity.Error);
                return false;
            }
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
            var result = await _httpRequests.SendDeleteRequestAsync<ResultModel>($"{RequestUri}/{id}");

            if (result.Success)
            {
                _logger.LogInformation($"DeleteAsync method executed successfully for id: {id}");
                _snackbar.Add("Туроператора успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні туроператора: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні туроператора: {ex.Message}", Severity.Error);
            throw;
        }
    }
}