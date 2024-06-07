using System.Net;
using CRM.Admin.Data.UserDTO;
using CRM.Admin.HttpRequests;
using Newtonsoft.Json;

namespace CRM.Admin.Requests.UserRequests;

public class UserRequest : IUserRequest
{
    private readonly IHttpCrmApiRequests _httpCrmApiRequests;
    private const string RequestUri = "api/User";
    
    public UserRequest(IHttpCrmApiRequests httpCrmApiRequests)
    {
        _httpCrmApiRequests = httpCrmApiRequests;
    }
    
    public async Task<List<UserDTO>> GetAllAsync()
    {
        var response = await _httpCrmApiRequests.SendGetRequestAsync(RequestUri);
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<UserDTO>>(content);
    }
    
    public async Task<T> GetByIdAsync<T>(Guid id) where T : IUserDTO
    {
        var response = await _httpCrmApiRequests.SendGetRequestAsync($"{RequestUri}/{id}");
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<T>(content);
    }
        
    public async Task<bool> UpdateAsync(UserUpdateDTO userUpdateDTO)
    {
        var response = await _httpCrmApiRequests.SendPutRequestAsync(RequestUri, userUpdateDTO);
        return response.StatusCode == HttpStatusCode.NoContent;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _httpCrmApiRequests.SendDeleteRequestAsync($"{RequestUri}/{id}");
        return response.StatusCode == HttpStatusCode.NoContent;
    }
}