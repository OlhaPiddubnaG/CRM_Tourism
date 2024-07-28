using CRM.Domain.Enums;

namespace CRM.Domain.Responses.ClientStatusHistory;

public class ClientStatusHistoryResponse
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; } 
    public DateTime DateTime { get; set; } 
    public ClientStatus ClientStatus { get; set; } 
}