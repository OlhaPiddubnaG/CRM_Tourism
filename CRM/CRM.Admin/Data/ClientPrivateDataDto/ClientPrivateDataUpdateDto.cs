namespace CRM.Admin.Data.ClientPrivateDataDto;

public class ClientPrivateDataUpdateDto : IClientPrivateDataDto
{
    public Guid Id { get; set; } 
    public Guid ClientId { get; set; } 
}