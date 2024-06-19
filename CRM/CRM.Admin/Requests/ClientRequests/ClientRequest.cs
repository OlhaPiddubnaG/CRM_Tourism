using System.Net;
using CRM.Admin.Data.ClientDTO;
using CRM.Admin.HttpRequests;
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
    
    public async Task<Guid> CreateAsync(ClientCreateDTO clientCreateDTO)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, clientCreateDTO);
            _logger.LogInformation("Create client method executed successfully");

            var createdClient = await response.Content.ReadFromJsonAsync<ClientDTO>();
            return createdClient.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
            throw;
        }
    }
    
    public async Task<List<ClientDTO>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<ClientDTO>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IClientDTO
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

    public async Task<bool> UpdateAsync(ClientUpdateDTO clientUpdateDTO)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, clientUpdateDTO);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"UpdateAsync method executed successfully for user with id: {clientUpdateDTO.Id}");
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {clientUpdateDTO.Id}");
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