using CRM.Admin.Data;
using CRM.Admin.Data.UserDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.UserRequests;

public class UserRequest : IUserRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<UserRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/User";

    public UserRequest(IHttpRequests httpRequests, ILogger<UserRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(UserCreateDto dto)
    {
        try
        {
            var createdUser = await _httpRequests.SendPostRequestAsync<UserDto>(RequestUri, dto);
            _logger.LogInformation("CreateUser method executed successfully");
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
            var result = await _httpRequests.SendGetRequestAsync<List<UserDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх користувачів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при отриманні всіх користувачів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<UserUpdateDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<UserUpdateDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані користувача успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при отриманні користувача з id {id}: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UserUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for user with id: {dto.Id}");
                _snackbar.Add("Користувач успішно оновлений", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"UpdateAsync method failed for user with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні користувача: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні користувача з id {dto.Id}: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Користувач успішно видалений", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні користувача з id {id}: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні користувача з id {id}: {ex.Message}", Severity.Error);
            throw;
        }
    }
}