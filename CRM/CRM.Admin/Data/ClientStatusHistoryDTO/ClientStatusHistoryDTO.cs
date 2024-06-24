using CRM.Domain.Enums;

namespace CRM.Admin.Data.ClientStatusHistoryDTO;

public class ClientStatusHistoryDTO
{
    public Guid Id { get; set; } 
    public Guid ClientId { get; set; } 
    public DateTime DateTime { get; set; } 
    public ClientStatus ClientStatus { get; set; } 
}