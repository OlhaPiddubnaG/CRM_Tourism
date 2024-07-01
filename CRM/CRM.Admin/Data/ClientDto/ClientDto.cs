using CRM.Domain.Enums;

namespace CRM.Admin.Data.ClientDto;

public class ClientDto : IClientDto
{
    public Guid Id { get; set; } 
    public Guid CompanyId { get; set; } 
    public Guid CountryId { get; set; } 
    public string Name { get; set; } = null!;
    public string? Surname { get; set; } 
    public string? Patronymic { get; set; } 
    public DateOnly? DateOfBirth { get; set; } 
    public Gender Gender { get; set; } 
    public string? Address { get; set; }
    public string? Email { get; set; }  
    public string? Phone { get; set; } 
    public string? Comment { get; set; } 
    public SourceOfEngagement SourceOfEngagement { get; set; } 
    public List<string> ManagerNames { get; set; } = new List<string>();
    public ClientStatus? LatestStatus { get; set; }
}