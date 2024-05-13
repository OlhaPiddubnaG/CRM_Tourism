using CRM.Domain.Entities.Base;
using CRM.Domain.Enums;

namespace CRM.Domain.Entities;

public class Client : Auditable
{
    public Guid CountryId { get; set; } 
    public Country? Country { get; set; } 
    public Guid CompanyId { get; set; } 
    public Company? Company { get; set; } 
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
    public List<Order> Orders { get; set; } = new();
    public List<ClientPrivateData> ClientPrivateDatas { get; set; } = new();
}