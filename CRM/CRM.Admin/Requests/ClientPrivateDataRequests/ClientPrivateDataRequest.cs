using System.Net;
using CRM.Admin.Data.ClientPrivateDataDTO;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.ClientPrivateDataRequests;

public class ClientPrivateDataRequest : IClientPrivateDataRequest
{
     private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<ClientPrivateDataRequest> _logger;
    private const string RequestUri = "api/ClientPrivateData";

    public ClientPrivateDataRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<ClientPrivateDataRequest> logger)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
    }
    
    public async Task<Guid> CreateAsync(ClientPrivateDataCreateDTO clientPrivateDataCreateDTO)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, clientPrivateDataCreateDTO);
            _logger.LogInformation("Create clientPrivateData method executed successfully");
            
            var createdClientPrivateData = await response.Content.ReadFromJsonAsync<ClientPrivateDataDTO>();
            return createdClientPrivateData.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
            throw;
        }
    }
    
    public async Task<List<ClientPrivateDataDTO>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<ClientPrivateDataDTO>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IClientPrivateDataDTO
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

    public async Task<bool> UpdateAsync(ClientPrivateDataUpdateDTO clientPrivateDataUpdateDTO)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, clientPrivateDataUpdateDTO);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"UpdateAsync method executed successfully for user with id: {clientPrivateDataUpdateDTO.Id}");
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {clientPrivateDataUpdateDTO.Id}");
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