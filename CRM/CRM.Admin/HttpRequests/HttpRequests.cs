using System.Net;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CRM.Admin.HttpRequests;

public class HttpRequests : IHttpCrmApiRequests
{ 
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navManager;

    public HttpRequests(IHttpClientFactory factory, NavigationManager navManager, IJSRuntime jsRuntime, string clientName)
    {
        _httpClient = factory.CreateClient(clientName);
        _navManager = navManager;
        _jsRuntime = jsRuntime;
    }

    public async Task<HttpResponseMessage> SendGetRequestAsync(string requestUri)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.GetAsync(requestUri);
        CheckResponseStatusCode(response);

        return response;
    }
    
    public async Task<HttpResponseMessage> SendGetRequestAsync(string requestUri, Dictionary<string, string> queryParams)
    {
        await AddAuthorizationHeaderAsync();

        var uriBuilder = new UriBuilder(_httpClient.BaseAddress + requestUri);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var param in queryParams)
        {
            query[param.Key] = param.Value;
        }

        uriBuilder.Query = query.ToString();
        var uri = uriBuilder.ToString();

        var response = await _httpClient.GetAsync(uri);
        CheckResponseStatusCode(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendPostRequestAsync<TValue>(string requestUri, TValue value)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync(requestUri, value);
        CheckResponseStatusCode(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendPutRequestAsync<TValue>(string requestUri, TValue value)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync(requestUri, value);
        CheckResponseStatusCode(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendDeleteRequestAsync(string requestUri)
    {
        await AddAuthorizationHeaderAsync();
        var response = await _httpClient.DeleteAsync(requestUri);
        CheckResponseStatusCode(response);

        return response;
    }
    
    private async Task AddAuthorizationHeaderAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
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
