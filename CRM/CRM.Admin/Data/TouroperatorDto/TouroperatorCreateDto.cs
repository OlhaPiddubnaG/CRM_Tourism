namespace CRM.Admin.Data.TouroperatorDto;

public class TouroperatorCreateDto
{
    public Guid? CompanyId { get; set; }
    public string Name { get; set; } = null!;
}