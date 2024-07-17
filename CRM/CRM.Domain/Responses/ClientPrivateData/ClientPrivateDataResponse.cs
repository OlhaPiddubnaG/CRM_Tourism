namespace CRM.Domain.Responses.ClientPrivateData;

public class ClientPrivateDataResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public bool IsDeleted { get; set; } 
}