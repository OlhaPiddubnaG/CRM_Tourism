using CRM.Domain.Enums;

namespace CRM.Admin.Data.ClientStatusHistoryDto;

public class ClientStatusHistoryCreateDto
{
    public Guid ClientId { get; set; } 
    public DateTime DateTime { get; set; } 
    public ClientStatus ClientStatus { get; set; } 
}