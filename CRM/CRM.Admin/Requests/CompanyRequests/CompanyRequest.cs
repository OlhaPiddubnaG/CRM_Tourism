using System.Net;
using CRM.Admin.Data.CompanyDto;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.CompanyRequests;

public class CompanyRequest : ICompanyRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private readonly ILogger<CompanyRequest> _logger;
    private const string RequestUri = "api/Company";

    public CompanyRequest(IHttpCrmApiRequests httpCrmApiRequests, ILogger<CompanyRequest> logger)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
        _logger = logger;
    }

    public async Task<List<CompanyDto>> GetAllAsync()
    {
        try
        {
            var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("GetAllAsync method executed successfully");
            return JsonConvert.DeserializeObject<List<CompanyDto>>(content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllAsync method");
            throw;
        }
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : ICompanyDto
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

    public async Task<bool> UpdateAsync(CompanyUpdateDto companyUpdateDto)
    {
        try
        {
            var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, companyUpdateDto);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation(
                $"UpdateAsync method executed successfully for company with id: {companyUpdateDto.Id}");
            return response.StatusCode == HttpStatusCode.NoContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error in UpdateAsync method for company with id: {companyUpdateDto.Id}");
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