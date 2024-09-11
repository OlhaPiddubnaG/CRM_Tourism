using CRM.Admin.Data;
using CRM.Admin.Data.UserTasksDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.UserTasksRequests;

public class UserTasksRequest : IUserTasksRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<UserTasksRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/UserTasks";

    public UserTasksRequest(IHttpRequests httpRequests, ILogger<UserTasksRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(UserTasksCreateDto dto)
    {
        try
        {
            var createdTask = await _httpRequests.SendPostRequestAsync<UserTasksDto>(RequestUri, dto);
            _logger.LogInformation("CreateAsync method executed successfully");
            _snackbar.Add("Задача успішно створена", Severity.Success);
            return createdTask.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateAsync method");
            _snackbar.Add($"Помилка при створенні задачі: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<UserTasksDto>> GetTasksByUserIdAsync(Guid userId)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<UserTasksDto>>($"{RequestUri}/user/{userId}");

            _logger.LogInformation("GetTasksByUserIdAsync method executed successfully");
            _snackbar.Add("Задачі користувача успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetTasksByUserIdAsync method");
            _snackbar.Add($"Помилка при отриманні задач користувача: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<UserTasksUpdateDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<UserTasksUpdateDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані задачі успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при отриманні задачі з id {id}: {ex.Message}", Severity.Error);
            throw;
        }
    }
    
    public async Task<List<UserTasksDto>> GetTasksByUserIdAndDateAsync(UserTasksRequestParameters parameters)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "userId", parameters.UserId.ToString()},
            { "dateTime", parameters.DateTime?.ToString("yyyy-MM-dd") ?? string.Empty } 
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        var pagedResponse =
            await _httpRequests.SendPostRequestAsync<List<UserTasksDto>>($"{RequestUri}/byDay?{queryString}",
                parameters);

        return pagedResponse;
    }

    public async Task<bool> UpdateAsync(UserTasksUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for task with id: {dto.Id}");
                _snackbar.Add("Задача успішно оновлена", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"UpdateAsync method failed for task with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні задачі: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for task with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні задачі з id {dto.Id}: {ex.Message}", Severity.Error);
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
                _logger.LogInformation($"DeleteAsync method executed successfully for task with id: {id}");
                _snackbar.Add("Задача успішно видалена", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for task with id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні задачі з id {id}: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for task with id: {id}");
            _snackbar.Add($"Помилка при видаленні задачі з id {id}: {ex.Message}", Severity.Error);
            throw;
        }
    }
}