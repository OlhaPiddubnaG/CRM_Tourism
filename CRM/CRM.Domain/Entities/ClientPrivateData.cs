using CRM.Domain.Entities.Base;

namespace CRM.Domain.Entities;

public class ClientPrivateData : Auditable
{
    public Guid ClientId { get; set; } 
    public Client? Client { get; set; } 
    public List<PassportInfo> PassportInfo { get; set; } = new(); 
}