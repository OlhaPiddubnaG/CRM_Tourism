namespace CRM.Admin.Data.TouroperatorDto;

public class TouroperatorUpdateDto : ITouroperatorDto
{
    public Guid Id { get; set; }
    public Guid? CompanyId { get; set; }
    public string Name { get; set; } = null!;
}