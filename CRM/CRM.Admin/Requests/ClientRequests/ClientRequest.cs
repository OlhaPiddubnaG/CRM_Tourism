using CRM.Admin.Data;
using CRM.Admin.Data.ClientDto;
using CRM.Admin.HttpRequests;
using MudBlazor;

namespace CRM.Admin.Requests.ClientRequests;

public class ClientRequest : IClientRequest
{
    private readonly IHttpRequests _httpRequests;
    private readonly ILogger<ClientRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Client";

    public ClientRequest(IHttpRequests httpRequests, ILogger<ClientRequest> logger, ISnackbar snackbar)
    {
        _httpRequests = httpRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(ClientCreateDto dto)
    {
        try
        {
            var createdClient = await _httpRequests.SendPostRequestAsync<ClientDto>(RequestUri, dto);
            _logger.LogInformation("Create client method executed successfully");
            _snackbar.Add("Клієнта успішно створено", Severity.Success);
            return createdClient.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
            _snackbar.Add($"Помилка при створенні клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<ResultModel> CreateClientWithRelatedAsync(ClientCreateDto dto)
    {
        try
        {
            var clientResponse =
                await _httpRequests.SendPostRequestAsync<ResultModel>($"{RequestUri}/withRelated", dto);

            if (clientResponse != null && clientResponse.Success)
            {
                _snackbar.Add("Клієнта та пов’язані дані успішно створено", Severity.Success);
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
            _logger.LogError(ex, "Error in CreateClientWithRelatedAsync method");
            _snackbar.Add($"Помилка при створенні клієнта з пов’язаними даними: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<List<ClientDto>> GetAllAsync()
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<List<ClientDto>>(RequestUri);

            _logger.LogInformation("GetAllAsync method executed successfully");
            _snackbar.Add("Дані всіх клієнтів успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            _snackbar.Add($"Помилка при завантаженні всіх клієнтів: {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<PagedResponse<ClientDto>> GetPagedDataAsync(ClientRequestParameters parameters)
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
            await _httpRequests.SendPostRequestAsync<PagedResponse<ClientDto>>($"{RequestUri}/paged?{queryString}",
                parameters);

        return pagedResponse;
    }

    public async Task<ClientUpdateDto> GetByIdAsync(Guid id)
    {
        try
        {
            var result = await _httpRequests.SendGetRequestAsync<ClientUpdateDto>($"{RequestUri}/{id}");

            _logger.LogInformation($"GetByIdAsync method executed successfully for id: {id}");
            _snackbar.Add("Дані клієнта успішно завантажено", Severity.Success);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByIdAsync method for id: {id}");
            _snackbar.Add($"Помилка при завантаженні клієнта з id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(ClientUpdateDto dto)
    {
        try
        {
            var result = await _httpRequests.SendPutRequestAsync<ResultModel>(RequestUri, dto);

            if (result.Success)
            {
                _logger.LogInformation($"UpdateAsync method executed successfully for client with id: {dto.Id}");
                _snackbar.Add("Клієнта успішно оновлено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning(
                    $"UpdateAsync method failed for client with id: {dto.Id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при оновленні клієнта: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for client with id: {dto.Id}");
            _snackbar.Add($"Помилка при оновленні клієнта: {ex.Message}", Severity.Error);
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
                _snackbar.Add("Клієнта успішно видалено", Severity.Success);
                return true;
            }
            else
            {
                _logger.LogWarning($"DeleteAsync method failed for id: {id}. Message: {result.Message}");
                _snackbar.Add($"Помилка при видаленні клієнта: {result.Message}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }
}