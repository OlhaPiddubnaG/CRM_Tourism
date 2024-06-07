using System.Net;
using CRM.Admin.Data.CompanyDTO;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.CompanyRequests;

public class CompanyRequest : ICompanyRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private const string RequestUri = "api/Company";
    
    public CompanyRequest(IHttpCrmApiRequests httpCrmApiRequests)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
    }
    
    public async Task<List<CompanyDTO>> GetAllAsync()
    {
        var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<CompanyDTO>>(content);
    }

    public async Task<T> GetByIdAsync<T>(Guid id) where T : ICompanyDTO
    {
        var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/{id}");
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(content);
    }
    
    public async Task<bool> UpdateAsync(CompanyUpdateDTO categotyUpdateDTO)
    {
        var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, categotyUpdateDTO);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _httpCrmApiRequests.SendDeleteRequestAsync($"{RequestUri}/{id}");
        return response.StatusCode == HttpStatusCode.NoContent;
    }
}