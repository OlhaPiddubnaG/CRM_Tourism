namespace CRM.Admin.Data.RoomTypeDto;

public class RoomTypeUpdateDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}