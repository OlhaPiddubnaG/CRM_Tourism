namespace CRM.Admin.Data.CountryDto;

public class CountryUpdateDto
{
    public Guid Id { get; set; } 
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } = null!;
}