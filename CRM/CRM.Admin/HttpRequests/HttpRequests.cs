using System.Net;
using Microsoft.AspNetCore.Components;

namespace CRM.Admin.HttpRequests;

public class HttpRequests : IHttpCrmApiRequests
{ 
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navManager;

    public HttpRequests(IHttpClientFactory factory, NavigationManager navManager, string clientName)
    {
        _httpClient = factory.CreateClient(clientName);
        _navManager = navManager;
    }

    public async Task<HttpResponseMessage> SendGetRequestAsync(string requestUri)
    {
        var response = await _httpClient.GetAsync(requestUri);

        CheckResponseStatusCode(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendPostRequestAsync<TValue>(string requestUri, TValue value)
    {
        var response = await _httpClient.PostAsJsonAsync(requestUri, value);

        CheckResponseStatusCode(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendPutRequestAsync<TValue>(string requestUri, TValue value)
    {
        var response = await _httpClient.PutAsJsonAsync(requestUri, value);

        CheckResponseStatusCode(response);

        return response;
    }

    public async Task<HttpResponseMessage> SendDeleteRequestAsync(string requestUri)
    {
        var response = await _httpClient.DeleteAsync(requestUri);

        CheckResponseStatusCode(response);

        return response;
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
