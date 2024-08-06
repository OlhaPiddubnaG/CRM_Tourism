using CRM.Admin.Data;
using CRM.Admin.Data.RoomTypeDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.RoomTypeRequests;

public class RoomTypeRequest : IRoomTypeRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<RoomTypeRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/RoomType";

    public RoomTypeRequest(IHttpRequests httpRequests, ILogger<RoomTypeRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(RoomTypeCreateDto dto)
    {
        try
        {
            var createdRoomType = await _httpRequests.SendPostRequestAsync<RoomTypeDto>(RequestUri, dto);
            _logger.LogInformation("CreateAsync method executed successfully");
            _snackbar.Add("Тип кімнати успішно створено", Severity.Success);

            return createdRoomType.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync method");
            _snackbar.Add($"Помилка при створенні типу кімнати: {ex.Message}", Severity.Error);
            throw;
        }
    }
    
    public async Task<List<RoomTypeDto>> GetFiltredDataAsync(string searchString)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "searchString", searchString ?? string.Empty }
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var response =
            await _httpRequests.SendPostRequestAsync<List<RoomTypeDto>>($"{RequestUri}/filter?{queryString}",
                searchString);

        return response;
    }

    public async Task<List<RoomTypeDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<RoomTypeDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх типів кімнат успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх типів кімнат: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(RoomTypeUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for room type with id: {dto.Id}");
                _snackbar.Add("Тип кімнати успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for room type with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні типу кімнати: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for room type with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні типу кімнати: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Тип кімнати успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні типу кімнати: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні типу кімнати: {ex.Message}", Severity.Error);
            throw;
        }
    }
}