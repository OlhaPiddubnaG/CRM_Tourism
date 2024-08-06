namespace CRM.Admin.Data.RoomTypeDto;

public class RoomTypeCreateDto
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}