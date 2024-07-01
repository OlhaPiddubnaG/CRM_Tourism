using System.Net;
using CRM.Admin.Data.PassportInfoDto;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.PassportInfoRequests;

public class PassportInfoRequest : IPassportInfoRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<PassportInfoRequest> _logger;
    private const string RequestUri = "api/PassportInfo";

    public PassportInfoRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<PassportInfoRequest> logger)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
    }
    
    public async Task<Guid> CreateAsync(PassportInfoCreateDto passportInfoCreateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPostRequestAsync(RequestUri, passportInfoCreateDto);
            _logger.LogInformation("Create client method executed successfully");
           
            var createdPassportInfo = await response.Content.ReadFromJsonAsync<PassportInfoDto>();
            return createdPassportInfo.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Create client method");
            throw;
        }
    }
    
    public async Task<List<PassportInfoDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<PassportInfoDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : IPassportInfoDto
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
    
    public async Task<List<PassportInfoDto>> GetByClientPrivateDataIdAsync(Guid clientPrivateDataId)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/by-client-private-data-id/{clientPrivateDataId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"GetByClientPrivateDataIdAsync method executed successfully for ClientPrivateDataId: {clientPrivateDataId}");
            return JsonConvert.DeserializeObject<List<PassportInfoDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in GetByClientPrivateDataIdAsync method for ClientPrivateDataId: {clientPrivateDataId}");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(PassportInfoUpdateDto passportInfoUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, passportInfoUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation($"UpdateAsync method executed successfully for user with id: {passportInfoUpdateDto.Id}");
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for user with id: {passportInfoUpdateDto.Id}");
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