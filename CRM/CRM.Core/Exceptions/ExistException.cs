namespace CRM.Core.Exceptions;

public class ExistException : Exception
{
    public ExistException(): base("Already exist")
    {
    }
}