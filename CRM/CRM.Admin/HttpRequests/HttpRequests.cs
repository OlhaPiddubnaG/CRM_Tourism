using System.Net;
using System.Net.Http.Headers;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace CRM.Admin.HttpRequests;

public class HttpRequests : IHttpRequests
{
    private readonly HttpClient _httpClient;
    private readonly ISessionStorageService _sessionStorage;
    private readonly NavigationManager _navManager;

    public HttpRequests(IHttpClientFactory factory, NavigationManager navManager, ISessionStorageService sessionStorage, 
        string clientName) 
    {
        _httpClient = factory.CreateClient(clientName);
        _navManager = navManager;
        _sessionStorage = sessionStorage;
    }

    public async Task<TResponse> SendGetRequestAsync<TResponse>(string requestUri)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync(requestUri);
        CheckResponseStatusCode(response);

        var content = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {content}");
    }

    public async Task<TResponse> SendPostRequestAsync<TResponse>(string requestUri, object value)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync(requestUri, value);
        CheckResponseStatusCode(response);

        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>();
        return responseContent;
    }

    public async Task<TResponse> SendPutRequestAsync<TResponse>(string requestUri, object value)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync(requestUri, value);
        CheckResponseStatusCode(response);

        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>();
        return responseContent;
    }

    public async Task<TResponse> SendDeleteRequestAsync<TResponse>(string requestUri)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync(requestUri);
        CheckResponseStatusCode(response);

        var responseContent = await response.Content.ReadFromJsonAsync<TResponse>();
        return responseContent;
    }

    private async Task AddAuthorizationHeaderAsync()
    {
        var token = await _sessionStorage.GetItemAsync<string>("token");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private void CheckResponseStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var statusCode = response.StatusCode;
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    _navManager.NavigateTo("/login");
                    break;
                default:
                    _navManager.NavigateTo("/error");
                    break;
            }
        }
    }
}