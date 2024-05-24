namespace CRM.Core.Exceptions;

public class AuthorizationFailedException : UnauthorizedAccessException
{
    public AuthorizationFailedException()
        : base("Email or Password is incorrect")
    {
    }
}