using CRM.Admin.Data;
using CRM.Admin.Data.MealsDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.MealsRequests;

public class MealsRequest : IMealsRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<MealsRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Meals";

    public MealsRequest(IHttpRequests httpRequests, ILogger<MealsRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(MealsCreateDto dto)
    {
        try
        {
            var createdMeal = await _httpRequests.SendPostRequestAsync<MealsDto>(RequestUri, dto);
            _logger.LogInformation("CreateAsync method executed successfully");
            _snackbar.Add("Тип харчування успішно створено", Severity.Success);

            return createdMeal.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync method");
            _snackbar.Add($"Помилка при створенні типу харчування: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<MealsDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<MealsDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх харчувань успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх харчувань: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<MealsDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<MealsDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані типу харчування успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні харчування за id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(MealsUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for meal with id: {dto.Id}");
                _snackbar.Add("Тип харчування успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"UpdateAsync method failed for meal with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні харчування: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for meal with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні харчування: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Харчування успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні харчування: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні харчування: {ex.Message}", Severity.Error);
            throw;
        }
    }
}