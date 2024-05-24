namespace CRM.Core.Exceptions;

public class SaveDatabaseException : Exception
{
    public SaveDatabaseException(Type objectType)
        : base($"Saving data error for entity of type {objectType}") {  }

    public SaveDatabaseException(Type objectType, Exception? exception)
        : base($"Saving data error for entity of type {objectType}", exception) { }
}