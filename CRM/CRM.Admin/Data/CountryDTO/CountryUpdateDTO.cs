namespace CRM.Admin.Data.CountryDTO;

public class CountryUpdateDTO : ICountryDTO
{
    public Guid Id { get; set; } 
    public Guid CompanyId { get; set; } 
    public string Name { get; set; } = null!;
}