namespace CRM.Admin.Data.ClientPrivateDataDTO;

public class ClientPrivateDataUpdateDTO : IClientPrivateDataDTO
{
    public Guid Id { get; set; } 
    public Guid ClientId { get; set; } 
}