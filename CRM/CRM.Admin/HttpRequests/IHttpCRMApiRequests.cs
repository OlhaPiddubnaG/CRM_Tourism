namespace CRM.Admin.HttpRequests;

public interface IHttpCrmApiRequests
{
    Task<HttpResponseMessage> SendGetRequestAsync(string requestUri);

    Task<HttpResponseMessage> SendPostRequestAsync<TValue>(string requestUri, TValue value);

    Task<HttpResponseMessage> SendPutRequestAsync<TValue>(string requestUri, TValue value);

    Task<HttpResponseMessage> SendDeleteRequestAsync(string requestUri);
}