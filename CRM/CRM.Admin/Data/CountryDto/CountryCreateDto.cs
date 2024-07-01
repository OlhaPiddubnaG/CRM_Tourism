namespace CRM.Admin.Data.CountryDto;

public class CountryCreateDto
{
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } = null!; 
}