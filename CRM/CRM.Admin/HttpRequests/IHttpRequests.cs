namespace CRM.Admin.HttpRequests;

public interface IHttpRequests
{
    Task<TResponse> SendGetRequestAsync<TResponse>(string requestUri);

    Task<TResponse> SendPostRequestAsync<TResponse>(string requestUri, object value);

    Task<TResponse> SendPutRequestAsync<TResponse>(string requestUri, object value);

    Task<TResponse> SendDeleteRequestAsync<TResponse>(string requestUri);
}