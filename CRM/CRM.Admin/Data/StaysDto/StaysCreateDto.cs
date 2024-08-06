namespace CRM.Admin.Data.StaysDto;

public class StaysCreateDto
{
    public Guid OrderId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime? CheckInDate { get; set; } = DateTime.UtcNow;
    public int NumberOfNights { get; set; }
    public string Comment { get; set; } = "";
}