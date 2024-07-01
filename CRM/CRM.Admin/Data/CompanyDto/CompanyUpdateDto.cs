namespace CRM.Admin.Data.CompanyDto;

public class CompanyUpdateDto : ICompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
}