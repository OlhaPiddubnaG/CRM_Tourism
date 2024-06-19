using CRM.Domain.Enums;

namespace CRM.Admin.Data.ClientDTO;

public class ClientCreateDTO
{
    public Guid CompanyId { get; set; } 
    public Guid CountryId { get; set; } = Guid.Empty;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = "";
    public string Patronymic { get; set; } = "";
    public DateOnly? DateOfBirth { get; set; } 
    public Gender Gender { get; set; } 
    public string Address { get; set; } = "";
    public string Email { get; set; } = "";  
    public string Phone { get; set; } = "";
    public string Comment { get; set; } = "";
    public SourceOfEngagement SourceOfEngagement { get; set; } 
}