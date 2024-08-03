using CRM.Admin.Data;
using CRM.Admin.Data.HotelDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.HotelRequests;

public class HotelRequest : IHotelRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<HotelRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Hotel";

    public HotelRequest(IHttpRequests httpRequests, ILogger<HotelRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(HotelCreateDto dto)
    {
        try
        {
            var createdHotel = await _httpRequests.SendPostRequestAsync<HotelDto>(RequestUri, dto);
            _logger.LogInformation("CreateAsync method executed successfully");
            _snackbar.Add("Готель успішно створено", Severity.Success);

            return createdHotel.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync method");
            _snackbar.Add($"Помилка при створенні готелю: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<HotelDto>> GetFiltredDataAsync(string searchString)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "searchString", searchString ?? string.Empty }
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var response =
            await _httpRequests.SendPostRequestAsync<List<HotelDto>>($"{RequestUri}/filter?{queryString}",
                searchString);

        return response;
    }

    public async Task<List<HotelDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<HotelDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх готелів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх готелів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<HotelDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<HotelDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані готелю успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні готелю за id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(HotelUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for hotel with id: {dto.Id}");
                _snackbar.Add("Готель успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for hotel with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні готелю: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for hotel with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні готелю: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Готель успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні готелю: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні готелю: {ex.Message}", Severity.Error);
            throw;
        }
    }
}