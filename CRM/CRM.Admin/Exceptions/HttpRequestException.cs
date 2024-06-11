using System.Net;

namespace CRM.Admin.Exceptions;

public class HttpRequestException : Exception
{

    public HttpRequestException(string message)
        : base(message)
    {
    }
}