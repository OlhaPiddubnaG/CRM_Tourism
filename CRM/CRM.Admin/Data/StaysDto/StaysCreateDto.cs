namespace CRM.Admin.Data.StaysDto;

public class StaysCreateDto
{
    public Guid OrderId { get; set; } 
    public string Name { get; set; } = null!;
    public DateTime CheckInDate { get; set; }
    public int NumberOfNights { get; set; }
    public string Comment { get; set; }
}