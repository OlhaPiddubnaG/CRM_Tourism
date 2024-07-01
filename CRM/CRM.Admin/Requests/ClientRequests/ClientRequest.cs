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
    private const string RequestUri = "api/Client";

    public ClientRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<ClientRequest> logger)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(ClientCreateDto clientCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, clientCreateDto);
            _logger.LogInformation("Create client method executed successfully");

            var createdClient = await response.Content.ReadFromJsonAsync<ClientDto>();
            return createdClient.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
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
                return new ResultModel
                {
                    Success = true,
                    Message = "Create successfully."
                };
            }
            else
            {
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
            throw;
        }
    }

    public async Task<TableData<ClientDto>> GetFilteredAndSortedAsync(int page, int pageSize, string searchString, string sortLabel, SortDirection sortDirection)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/search?page={page}&pageSize={pageSize}&searchString={searchString}&sortLabel={sortLabel}&sortDirection={sortDirection}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TableData<ClientDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetFilteredAndSortedAsync method");
            throw;
        }
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
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {clientUpdateDto.Id}");
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
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in DeleteAsync method for id: {id}");
            throw;
        }
    }
}