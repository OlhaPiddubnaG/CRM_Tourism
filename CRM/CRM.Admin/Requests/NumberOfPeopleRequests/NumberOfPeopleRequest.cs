using CRM.Admin.Data;
using CRM.Admin.Data.NumberOfPeopleDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.NumberOfPeopleRequests;

public class NumberOfPeopleRequest : INumberOfPeopleRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<NumberOfPeopleRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/NumberOfPeople";

    public NumberOfPeopleRequest(IHttpRequests httpRequests, ILogger<NumberOfPeopleRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(NumberOfPeopleCreateDto dto)
    {
        try
        {
            var createdOrder = await _httpRequests.SendPostRequestAsync<NumberOfPeopleDto>(RequestUri, dto);
            _logger.LogInformation("CreateAsync method executed successfully");
            _snackbar.Add("Кількість людей успішно створено", Severity.Success);

            return createdOrder.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync method");
            _snackbar.Add($"Помилка при створенні кількості людей: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<NumberOfPeopleDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<NumberOfPeopleDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх кількостей людей успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх кількостей людей: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<NumberOfPeopleDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<NumberOfPeopleDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані кількості людей успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні кількості людей за id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(NumberOfPeopleUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for order with id: {dto.Id}");
                _snackbar.Add("Кількість людей успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"UpdateAsync method failed for order with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні кількості людей: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for order with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні кількості людей: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Кількість людей успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні кількості людей: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні кількості людей: {ex.Message}", Severity.Error);
            throw;
        }
    }
}