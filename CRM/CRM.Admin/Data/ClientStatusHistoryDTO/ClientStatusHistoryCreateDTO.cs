using CRM.Domain.Enums;

namespace CRM.Admin.Data.ClientStatusHistoryDTO;

public class ClientStatusHistoryCreateDTO
{
    public Guid ClientId { get; set; } 
    public DateTime DateTime { get; set; } 
    public ClientStatus ClientStatus { get; set; } 
}