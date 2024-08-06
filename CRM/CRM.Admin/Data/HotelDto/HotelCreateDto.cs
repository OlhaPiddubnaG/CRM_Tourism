namespace CRM.Admin.Data.HotelDto;

public class HotelCreateDto
{
    public Guid CompanyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
}