namespace CRM.Admin.Data.CountryDTO;

public class CountryCreateDTO
{
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } = null!; 
}