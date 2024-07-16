using System.Net;
using CRM.Admin.Data;
using CRM.Admin.Data.ClientDto;
using CRM.Admin.HttpRequests;
using MudBlazor;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.ClientRequests;

public class ClientRequest : IClientRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<ClientRequest> _logger;
    private readonly ISnackbar _snackbar;
    private const string RequestUri = "api/Client";

    public ClientRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<ClientRequest> logger, ISnackbar snackbar)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
        _snackbar = snackbar;
    }

    public async Task<Guid> CreateAsync(ClientCreateDto clientCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, clientCreateDto);
            _logger.LogInformation("Create client method executed successfully");

            var createdClient = await response.Content.ReadFromJsonAsync<ClientDto>();
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

    public async Task<ResultModel> CreateClientWithRelatedAsync(ClientCreateDto clientCreateDto)
    {
        try
        {
            var clientResponse =
                await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/withRelated", clientCreateDto);
            await clientResponse.Content.ReadAsStringAsync();

            if (clientResponse.IsSuccessStatusCode)
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
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<ClientDto>>(content);
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
        var response = await _httpCrmApiRequests.SendPostRequestAsync($"{RequestUri}/paged?{queryString}", parameters);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PagedResponse<ClientDto>>();
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IClientDto
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
            _snackbar.Add($"Помилка при завантаженні клієнта з id: {id}, {ex.Message}", Severity.Error);
            throw;
        }
    }

    public async Task<bool> UpdateAsync(ClientUpdateDto clientUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, clientUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"UpdateAsync method executed successfully for user with id: {clientUpdateDto.Id}");
            _snackbar.Add("Клієнта успішно оновлено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {clientUpdateDto.Id}");
            _snackbar.Add($"Помилка при оновленні клієнта: {ex.Message}", Severity.Error);
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
            _snackbar.Add("Клієнта успішно видалено", Severity.Success);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            _snackbar.Add($"Помилка при видаленні клієнта: {ex.Message}", Severity.Error);
            throw;
        }
    }
}