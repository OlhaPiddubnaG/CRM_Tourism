namespace CRM.Admin.Data.CountryDTO;

public class CountryDTO
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } 
    public bool IsDeleted { get; set; } 
}