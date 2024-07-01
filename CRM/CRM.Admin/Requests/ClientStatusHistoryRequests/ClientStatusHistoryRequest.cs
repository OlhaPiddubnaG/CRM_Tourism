using System.Net;
using CRM.Admin.Data.ClientStatusHistoryDto;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.ClientStatusHistoryRequests;

public class ClientStatusHistoryRequest : IClientStatusHistoryRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<ClientStatusHistoryRequest> _logger;
    private const string RequestUri = "api/ClientStatusHistory";

    public ClientStatusHistoryRequest(IHttpCrmApiRequests httpCrmApiRequests,
        ILogger<ClientStatusHistoryRequest> logger)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(ClientStatusHistoryCreateDto clientStatusHistoryCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, clientStatusHistoryCreateDto);
            _logger.LogInformation("Create ClientStatusHistory method executed successfully");

            var createdClientStatusHistory = await response.Content.ReadFromJsonAsync<ClientStatusHistoryDto>();
            return createdClientStatusHistory.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
            throw;
        }
    }

    public async Task<List<ClientStatusHistoryDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<ClientStatusHistoryDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IClientStatusHistoryDto
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
}