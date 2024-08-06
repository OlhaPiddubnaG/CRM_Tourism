namespace CRM.Admin.Data.RoomTypeDto;

public class RoomTypeDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}