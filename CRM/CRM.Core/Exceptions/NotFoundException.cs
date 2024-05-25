namespace CRM.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(Type objectType, Guid Id)
        : base($"Object of type {objectType} with {Id} not found") {  }
    
    public NotFoundException(Type objectType)
        : base($"Object of type {objectType} with  not found") { }
}