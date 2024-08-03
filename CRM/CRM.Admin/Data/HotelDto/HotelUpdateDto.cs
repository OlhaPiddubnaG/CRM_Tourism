namespace CRM.Admin.Data.HotelDto;

public class HotelUpdateDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
}