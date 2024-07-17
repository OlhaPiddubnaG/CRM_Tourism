using System.Net;
using CRM.Admin.Data.UserDto;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.UserRequests;

public class UserRequest : IUserRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<UserRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/User";

    public UserRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<UserRequest> logger, ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(UserCreateDto userCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, userCreateDto);
            _logger.LogInformation("CreateUser method executed successfully");
            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
            _snackbar.Add("Користувач успішно створений", Severity.Success);
            return createdUser.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateUser method");
            _snackbar.Add($"Помилка при створенні користувача: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<UserDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при отриманні всіх користувачів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IUserDto
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
            _snackbar.Add($"Помилка при отриманні користувача з id {id}: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UserUpdateDto userUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, userUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"UpdateAsync method executed successfully for user with id: {userUpdateDto.Id}");
            _snackbar.Add("Користувач успішно оновлений", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {userUpdateDto.Id}");
            _snackbar.Add($"Помилка при оновленні користувача з id {userUpdateDto.Id}: {ex.Message}", Severity.Error);
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
            _snackbar.Add("Користувач успішно видалений", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні користувача з id {id}: {ex.Message}", Severity.Error);
            throw;
        }
    }
}