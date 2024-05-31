namespace CRM.Core.Exceptions;

public class RegistrationFailedException : Exception
{
    public object RelevantObject { get; }

    public RegistrationFailedException(string message, object relevantObject)
        : base(message)
    {
        RelevantObject = relevantObject;
    }

    public RegistrationFailedException(string message, Exception innerException, object relevantObject)
        : base(message, innerException)
    {
        RelevantObject = relevantObject;
    }
}