namespace CRM.Admin.Data.CountryDto;

public class CountryDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } 
    public bool IsDeleted { get; set; } 
}